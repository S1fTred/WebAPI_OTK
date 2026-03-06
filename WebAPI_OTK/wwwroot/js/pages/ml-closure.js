// ========================================
// МОДУЛЬ ЗАКРЫТИЯ МЛ
// ========================================

// Глобальное состояние
let currentMlList = [];
let currentMl = null;
let currentOperations = [];
let filteredOperations = [];
let employees = [];
let products = [];

// ========================================
// ИНИЦИАЛИЗАЦИЯ
// ========================================

document.addEventListener('DOMContentLoaded', async () => {
    // Проверка авторизации
    if (!checkAuth()) return;
    
    // Загрузка справочников
    await loadReferenceData();
    
    // Инициализация обработчиков
    initEventHandlers();
    
    // Обработчик выхода
    document.getElementById('logoutBtn').addEventListener('click', logout);
});

// Загрузка справочных данных
async function loadReferenceData() {
    try {
        // Загрузка сотрудников
        try {
            employees = await employeeApi.getAll();
        } catch (error) {
            console.warn('Не удалось загрузить сотрудников:', error);
            employees = [];
        }
        
        // Загрузка изделий
        try {
            products = await productApi.getAll();
        } catch (error) {
            console.warn('Не удалось загрузить изделия:', error);
            products = [];
        }
        
        // Заполнение селектов
        populateProductSelect();
        populateEmployeeSelects();
        
        // Показываем предупреждение, если данных нет
        if (employees.length === 0 && products.length === 0) {
            showToast('База данных пуста. Добавьте данные через API или SQL.', 'warning');
        }
        
    } catch (error) {
        console.error('Критическая ошибка загрузки справочников:', error);
        showToast('Не удалось подключиться к серверу. Проверьте, что сервер запущен.', 'error');
    }
}

// Заполнение селекта изделий
function populateProductSelect() {
    const select = document.getElementById('productSelect');
    select.innerHTML = '<option value="">Выберите изделие...</option>';
    
    products.forEach(product => {
        const option = document.createElement('option');
        option.value = product.id;
        option.textContent = product.наименование;
        select.appendChild(option);
    });
}

// Заполнение селектов сотрудников
function populateEmployeeSelects() {
    // Для фильтра операций
    const filterSelect = document.getElementById('operationEmployeeFilter');
    filterSelect.innerHTML = '<option value="">Все сотрудники</option>';
    
    // Для закрытия МЛ
    const otkSelect = document.getElementById('otkEmployee');
    otkSelect.innerHTML = '<option value="">Выберите сотрудника...</option>';
    
    employees.forEach(emp => {
        const option1 = document.createElement('option');
        option1.value = emp.id;
        option1.textContent = emp.фио;
        filterSelect.appendChild(option1);
        
        const option2 = document.createElement('option');
        option2.value = emp.id;
        option2.textContent = emp.фио;
        otkSelect.appendChild(option2);
    });
}

// ========================================
// ОБРАБОТЧИКИ СОБЫТИЙ
// ========================================

function initEventHandlers() {
    // Форма поиска
    document.getElementById('searchForm').addEventListener('submit', handleSearch);
    
    // Переключение типа поиска
    document.getElementById('searchType').addEventListener('change', handleSearchTypeChange);
    
    // Изменение изделия - загрузка ДСЕ
    document.getElementById('productSelect').addEventListener('change', handleProductChange);
    
    // Кнопки управления
    document.getElementById('closeMlBtn').addEventListener('click', () => showModal('closeMlModal'));
    document.getElementById('backToListBtn').addEventListener('click', backToList);
    
    // Форма закрытия МЛ
    document.getElementById('closeMlForm').addEventListener('submit', handleCloseMl);
    
    // Фильтры операций
    document.getElementById('operationStatusFilter').addEventListener('change', applyOperationFilters);
    document.getElementById('operationEmployeeFilter').addEventListener('change', applyOperationFilters);
    document.getElementById('operationDateFrom').addEventListener('change', applyOperationFilters);
    document.getElementById('operationDateTo').addEventListener('change', applyOperationFilters);
    
    // Закрытие модального окна
    document.querySelector('#closeMlModal .modal-close').addEventListener('click', () => {
        hideModal('closeMlModal');
    });
}

// Обработка изменения типа поиска
function handleSearchTypeChange(e) {
    const type = e.target.value;
    
    // Скрыть все группы
    document.getElementById('mlNumberGroup').classList.add('hidden');
    document.getElementById('productGroup').classList.add('hidden');
    document.getElementById('dceGroup').classList.add('hidden');
    
    // Показать нужную группу
    if (type === 'ml') {
        document.getElementById('mlNumberGroup').classList.remove('hidden');
    } else if (type === 'product') {
        document.getElementById('productGroup').classList.remove('hidden');
    } else if (type === 'dce') {
        document.getElementById('dceGroup').classList.remove('hidden');
    }
}

// Обработка изменения изделия
async function handleProductChange(e) {
    const productId = e.target.value;
    if (!productId) {
        document.getElementById('dceGroup').classList.add('hidden');
        return;
    }
    
    try {
        const dceList = await productApi.getDCE(productId);
        const dceSelect = document.getElementById('dceSelect');
        dceSelect.innerHTML = '<option value="">Выберите ДСЕ...</option>';
        
        dceList.forEach(dce => {
            const option = document.createElement('option');
            option.value = dce.id;
            option.textContent = `${dce.код} - ${dce.наименование || ''}`;
            dceSelect.appendChild(option);
        });
        
    } catch (error) {
        console.error('Ошибка загрузки ДСЕ:', error);
        showToast('Ошибка загрузки ДСЕ', 'error');
    }
}

// ========================================
// ПОИСК МЛ
// ========================================

async function handleSearch(e) {
    e.preventDefault();
    
    const searchType = document.getElementById('searchType').value;
    const statusFilter = document.getElementById('statusFilter').value;
    
    const filters = {};
    
    // Фильтр по статусу
    if (statusFilter === 'open') {
        filters.открытые = true;
    } else if (statusFilter === 'closed') {
        filters.открытые = false;
    }
    
    // Фильтры по типу поиска
    if (searchType === 'product') {
        const productId = document.getElementById('productSelect').value;
        if (!productId) {
            showToast('Выберите изделие', 'warning');
            return;
        }
        filters.изделиеId = productId;
    } else if (searchType === 'dce') {
        const dceId = document.getElementById('dceSelect').value;
        if (!dceId) {
            showToast('Выберите ДСЕ', 'warning');
            return;
        }
        filters.дсеId = dceId;
    }
    
    try {
        let mlList = await mlApi.getAll(filters);
        
        // Фильтр по номеру МЛ (клиентская фильтрация)
        if (searchType === 'ml') {
            const mlNumber = document.getElementById('mlNumber').value.trim();
            if (mlNumber) {
                mlList = mlList.filter(ml => 
                    ml.номерМЛ && ml.номерМЛ.toLowerCase().includes(mlNumber.toLowerCase())
                );
            }
        }
        
        currentMlList = mlList;
        displayMlList(mlList);
        
    } catch (error) {
        console.error('Ошибка поиска МЛ:', error);
        showToast('Ошибка при поиске маршрутных листов', 'error');
    }
}

// Отображение списка МЛ
function displayMlList(mlList) {
    const section = document.getElementById('mlListSection');
    const container = document.getElementById('mlListContainer');
    const countBadge = document.getElementById('mlCount');
    
    countBadge.textContent = mlList.length;
    
    if (mlList.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <div class="empty-state-icon">📋</div>
                <h3>Маршрутные листы не найдены</h3>
                <p>Попробуйте изменить параметры поиска</p>
            </div>
        `;
        section.classList.remove('hidden');
        return;
    }
    
    const columns = [
        { label: 'Номер МЛ', field: 'номерМЛ' },
        { label: 'Изделие', field: 'изделие' },
        { label: 'ДСЕ', field: 'дсе' },
        { label: 'Дата создания', field: 'датаСоздания', format: (val) => formatDate(val) },
        { 
            label: 'Статус', 
            field: 'закрыт',
            format: (val) => {
                const status = val ? 'закрыт' : 'открыт';
                const text = val ? 'Закрыт' : 'Открыт';
                return `<span class="status-badge ${status}">${text}</span>`;
            }
        },
        { label: 'Операций', field: 'количествоОпераций' }
    ];
    
    const tableHtml = createTable(columns, mlList, {
        emptyMessage: 'Нет маршрутных листов',
        onRowClick: (index) => selectMl(mlList[index])
    });
    
    container.innerHTML = tableHtml;
    
    // Добавляем обработчики кликов на строки
    const rows = container.querySelectorAll('tbody tr');
    rows.forEach((row, index) => {
        row.classList.add('table-row-clickable');
        row.addEventListener('click', () => selectMl(mlList[index]));
    });
    
    section.classList.remove('hidden');
}

// ========================================
// ДЕТАЛИ МЛ И ОПЕРАЦИИ
// ========================================

async function selectMl(ml) {
    currentMl = ml;
    
    try {
        // Загрузка операций МЛ
        const operations = await operationApi.getAll({ млId: ml.id });
        currentOperations = operations;
        filteredOperations = operations;
        
        // Отображение деталей МЛ
        displayMlDetails(ml);
        
        // Отображение операций
        displayOperations(operations);
        
        // Показать секцию деталей
        document.getElementById('mlDetailsSection').classList.remove('hidden');
        
        // Скрыть список МЛ
        document.getElementById('mlListSection').classList.add('hidden');
        
        // Прокрутка к деталям
        document.getElementById('mlDetailsSection').scrollIntoView({ behavior: 'smooth' });
        
    } catch (error) {
        console.error('Ошибка загрузки операций:', error);
        showToast('Ошибка загрузки операций МЛ', 'error');
    }
}

// Отображение деталей МЛ
function displayMlDetails(ml) {
    document.getElementById('selectedMlNumber').textContent = ml.номерМЛ || '-';
    document.getElementById('mlProduct').textContent = ml.изделие || '-';
    document.getElementById('mlDce').textContent = ml.дсе || '-';
    document.getElementById('mlDateCreated').textContent = formatDate(ml.датаСоздания);
    
    const statusHtml = ml.закрыт 
        ? '<span class="status-badge закрыт">Закрыт</span>'
        : '<span class="status-badge открыт">Открыт</span>';
    document.getElementById('mlStatus').innerHTML = statusHtml;
    
    document.getElementById('mlEmployee').textContent = ml.сотрудникОТК || '-';
    document.getElementById('mlOperationCount').textContent = ml.количествоОпераций || 0;
    
    // Кнопка закрытия МЛ
    const closeMlBtn = document.getElementById('closeMlBtn');
    closeMlBtn.disabled = ml.закрыт === true;
}

// Отображение операций
function displayOperations(operations) {
    const container = document.getElementById('operationsContainer');
    
    if (operations.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <div class="empty-state-icon">⚙️</div>
                <h3>Нет операций</h3>
                <p>Для этого МЛ еще не добавлены операции</p>
            </div>
        `;
        return;
    }
    
    const columns = [
        { label: 'Тип операции', field: 'типОперации' },
        { label: 'Сотрудник', field: 'сотрудник' },
        { label: 'Дата начала', field: 'датаНачала', format: (val) => formatDateTime(val) },
        { label: 'Дата окончания', field: 'датаОкончания', format: (val) => formatDateTime(val) },
        { label: 'Длительность (ч)', field: 'фактическаяДлительностьЧас', format: (val) => formatNumber(val, 2) },
        { 
            label: 'Статус', 
            field: 'статус',
            format: (val) => {
                if (!val) return '-';
                const statusClass = val.toLowerCase().replace(' ', '-');
                return `<span class="status-badge ${statusClass}">${val}</span>`;
            }
        }
    ];
    
    const rowActions = [
        {
            label: 'Завершить',
            class: 'btn-sm btn-success',
            handler: (index) => finishOperation(operations[index]),
            disabled: (row) => row.статус === 'Завершена'
        }
    ];
    
    const tableHtml = createTable(columns, operations, {
        emptyMessage: 'Нет операций',
        rowActions: rowActions
    });
    
    container.innerHTML = tableHtml;
}

// Применение фильтров к операциям
function applyOperationFilters() {
    const statusFilter = document.getElementById('operationStatusFilter').value;
    const employeeFilter = document.getElementById('operationEmployeeFilter').value;
    const dateFrom = document.getElementById('operationDateFrom').value;
    const dateTo = document.getElementById('operationDateTo').value;
    
    let filtered = [...currentOperations];
    
    // Фильтр по статусу
    if (statusFilter) {
        filtered = filtered.filter(op => op.статус === statusFilter);
    }
    
    // Фильтр по сотруднику
    if (employeeFilter) {
        filtered = filtered.filter(op => op.сотрудникID === parseInt(employeeFilter));
    }
    
    // Фильтр по дате начала
    if (dateFrom) {
        const fromDate = new Date(dateFrom);
        filtered = filtered.filter(op => new Date(op.датаНачала) >= fromDate);
    }
    
    if (dateTo) {
        const toDate = new Date(dateTo);
        toDate.setHours(23, 59, 59);
        filtered = filtered.filter(op => new Date(op.датаНачала) <= toDate);
    }
    
    filteredOperations = filtered;
    displayOperations(filtered);
}

// ========================================
// ДЕЙСТВИЯ С ОПЕРАЦИЯМИ И МЛ
// ========================================

// Завершение операции
async function finishOperation(operation) {
    confirmDialog(
        `Завершить операцию "${operation.типОперации}"?`,
        async () => {
            try {
                await operationApi.finish(operation.id);
                showToast('Операция успешно завершена', 'success');
                
                // Перезагрузка операций
                await selectMl(currentMl);
                
            } catch (error) {
                console.error('Ошибка завершения операции:', error);
                showToast('Ошибка при завершении операции', 'error');
            }
        }
    );
}

// Закрытие МЛ
async function handleCloseMl(e) {
    e.preventDefault();
    
    const employeeId = parseInt(document.getElementById('otkEmployee').value);
    const quantity = parseInt(document.getElementById('otkQuantity').value);
    const defects = parseInt(document.getElementById('defectQuantity').value);
    
    if (!employeeId) {
        showToast('Выберите сотрудника ОТК', 'warning');
        return;
    }
    
    const data = {
        сотрудникОТКID: employeeId,
        количествоОТК: quantity,
        количествоБрак: defects
    };
    
    try {
        await mlApi.close(currentMl.id, data);
        showToast('Маршрутный лист успешно закрыт', 'success');
        
        hideModal('closeMlModal');
        
        // Обновление данных
        currentMl.закрыт = true;
        displayMlDetails(currentMl);
        
        // Очистка формы
        document.getElementById('closeMlForm').reset();
        
    } catch (error) {
        console.error('Ошибка закрытия МЛ:', error);
        showToast(error.message || 'Ошибка при закрытии МЛ', 'error');
    }
}

// Возврат к списку МЛ
function backToList() {
    document.getElementById('mlDetailsSection').classList.add('hidden');
    document.getElementById('mlListSection').classList.remove('hidden');
    
    // Сброс фильтров операций
    document.getElementById('operationStatusFilter').value = '';
    document.getElementById('operationEmployeeFilter').value = '';
    document.getElementById('operationDateFrom').value = '';
    document.getElementById('operationDateTo').value = '';
    
    currentMl = null;
    currentOperations = [];
    filteredOperations = [];
}
