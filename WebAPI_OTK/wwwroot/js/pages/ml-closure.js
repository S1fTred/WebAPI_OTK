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

// Состояние пагинации
let currentPage = 1;
let pageSize = 10;
let totalPages = 1;
let totalCount = 0;

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
            showToast(t('message.emptyDatabase'), 'warning');
        }
        
    } catch (error) {
        console.error('Критическая ошибка загрузки справочников:', error);
        showToast(t('message.serverError'), 'error');
    }
}

// Заполнение селекта изделий
function populateProductSelect() {
    const select = document.getElementById('productSelect');
    select.innerHTML = `<option value="">${t('mlClosure.selectProduct')}</option>`;
    
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
    filterSelect.innerHTML = `<option value="">${t('mlClosure.allEmployees')}</option>`;
    
    // Для закрытия МЛ
    const otkSelect = document.getElementById('otkEmployee');
    otkSelect.innerHTML = `<option value="">${t('modal.closeMl.selectEmployee')}</option>`;
    
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
    
    // Экспорт конкретного МЛ
    document.getElementById('exportMlPdfBtn').addEventListener('click', () => {
        if (currentMl && currentMl.id) {
            console.log('Exporting ML detail:', currentMl.id);
            exportApi.exportRouteListDetailToPDF(currentMl.id);
        } else {
            console.error('No ML selected');
            showToast('Маршрутный лист не выбран', 'error');
        }
    });
    
    // Форма закрытия МЛ
    document.getElementById('closeMlForm').addEventListener('submit', handleCloseMl);
    
    // Фильтры операций
    document.getElementById('operationStatusFilter').addEventListener('change', applyOperationFilters);
    document.getElementById('operationEmployeeFilter').addEventListener('change', applyOperationFilters);
    document.getElementById('operationDateFrom').addEventListener('change', applyOperationFilters);
    document.getElementById('operationDateTo').addEventListener('change', applyOperationFilters);
    
    // Экспорт
    const exportExcelBtn = document.getElementById('exportExcelBtn');
    const exportPdfBtn = document.getElementById('exportPdfBtn');
    
    console.log('Export buttons:', { exportExcelBtn, exportPdfBtn });
    console.log('exportApi available:', typeof exportApi !== 'undefined');
    
    if (exportExcelBtn) {
        exportExcelBtn.addEventListener('click', () => {
            console.log('Export Excel clicked');
            if (typeof exportApi === 'undefined') {
                console.error('exportApi is not defined');
                showToast('Ошибка: модуль экспорта не загружен', 'error');
                return;
            }
            const filters = getCurrentSearchFilters();
            console.log('Filters:', filters);
            exportApi.exportRouteListsToExcel(filters);
        });
    } else {
        console.error('exportExcelBtn not found');
    }
    
    if (exportPdfBtn) {
        exportPdfBtn.addEventListener('click', () => {
            console.log('Export PDF clicked');
            if (typeof exportApi === 'undefined') {
                console.error('exportApi is not defined');
                showToast('Ошибка: модуль экспорта не загружен', 'error');
                return;
            }
            const filters = getCurrentSearchFilters();
            console.log('Filters:', filters);
            exportApi.exportRouteListsToPDF(filters);
        });
    } else {
        console.error('exportPdfBtn not found');
    }
    
    // Закрытие модального окна
    document.querySelector('#closeMlModal .modal-close').addEventListener('click', () => {
        hideModal('closeMlModal');
    });
    
    // Закрытие модального окна расчета
    document.querySelector('#calculationModal .modal-close').addEventListener('click', () => {
        hideModal('calculationModal');
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
        dceSelect.innerHTML = `<option value="">${t('mlClosure.selectDce')}</option>`;
        
        dceList.forEach(dce => {
            const option = document.createElement('option');
            option.value = dce.id;
            option.textContent = `${dce.код} - ${dce.наименование || ''}`;
            dceSelect.appendChild(option);
        });
        
    } catch (error) {
        console.error('Ошибка загрузки ДСЕ:', error);
        showToast(t('message.errorLoadingDce'), 'error');
    }
}

// ========================================
// ПОИСК МЛ
// ========================================

async function handleSearch(e) {
    e.preventDefault();
    
    // Сброс на первую страницу при новом поиске
    currentPage = 1;
    
    await loadMlList();
}

async function loadMlList() {
    const searchType = document.getElementById('searchType').value;
    const statusFilter = document.getElementById('statusFilter').value;
    
    const filters = {
        page: currentPage,
        pageSize: pageSize
    };
    
    // Фильтр по статусу
    if (statusFilter === 'open') {
        filters.открытые = true;
    } else if (statusFilter === 'closed') {
        filters.открытые = false;
    }
    
    // Фильтры по типу поиска
    if (searchType === 'ml') {
        const mlNumber = document.getElementById('mlNumber').value.trim();
        if (mlNumber) {
            filters.search = mlNumber;
        }
    } else if (searchType === 'product') {
        const productId = document.getElementById('productSelect').value;
        if (!productId) {
            showToast(t('message.selectProduct'), 'warning');
            return;
        }
        filters.изделиеId = productId;
    } else if (searchType === 'dce') {
        const dceId = document.getElementById('dceSelect').value;
        if (!dceId) {
            showToast(t('message.selectDce'), 'warning');
            return;
        }
        filters.дсеId = dceId;
    }
    
    try {
        console.log('Загрузка МЛ с фильтрами:', filters);
        const response = await mlApi.getAll(filters);
        console.log('Ответ API:', response);
        
        // Обработка ответа с пагинацией
        if (response && response.data && Array.isArray(response.data)) {
            currentMlList = response.data;
            totalCount = response.totalCount || 0;
            totalPages = response.totalPages || 1;
            currentPage = response.page || 1;
            pageSize = response.pageSize || 10;
        } else if (Array.isArray(response)) {
            // Обратная совместимость (если API вернул массив)
            currentMlList = response;
            totalCount = currentMlList.length;
            totalPages = 1;
        } else {
            console.error('Неожиданный формат ответа:', response);
            currentMlList = [];
            totalCount = 0;
            totalPages = 1;
        }
        
        console.log('Обработанные данные:', { currentMlList, totalCount, totalPages, currentPage });
        displayMlList(currentMlList);
        
    } catch (error) {
        console.error('Ошибка поиска МЛ:', error);
        showToast(t('message.errorSearchMl'), 'error');
    }
}

// Изменение страницы
function changePage(newPage) {
    if (newPage < 1 || newPage > totalPages) return;
    currentPage = newPage;
    loadMlList();
}

// Изменение размера страницы
function changePageSize(newSize) {
    pageSize = newSize;
    currentPage = 1; // Сброс на первую страницу
    loadMlList();
}

// Отображение списка МЛ
function displayMlList(mlList) {
    const section = document.getElementById('mlListSection');
    const container = document.getElementById('mlListContainer');
    const countBadge = document.getElementById('mlCount');
    
    // Удаляем старую пагинацию если есть
    const oldPagination = container.nextElementSibling;
    if (oldPagination && oldPagination.classList.contains('pagination')) {
        oldPagination.remove();
    }
    
    // Обновляем счетчик с информацией о пагинации
    const startRecord = mlList.length > 0 ? (currentPage - 1) * pageSize + 1 : 0;
    const endRecord = Math.min(currentPage * pageSize, totalCount);
    countBadge.textContent = `${startRecord}-${endRecord} ${t('pagination.of')} ${totalCount}`;
    
    if (mlList.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <div class="empty-state-icon">📋</div>
                <h3>${t('message.noMl')}</h3>
                <p>${t('message.tryChangeFilters')}</p>
            </div>
        `;
        section.classList.remove('hidden');
        return;
    }
    
    const columns = [
        { label: t('table.mlNumber'), field: 'номерМЛ' },
        { label: t('table.product'), field: 'изделие' },
        { label: t('table.dce'), field: 'дсе' },
        { label: t('table.dateCreated'), field: 'датаСоздания', format: (val) => formatDate(val) },
        { 
            label: t('table.status'), 
            field: 'закрыт',
            format: (val) => {
                const status = val ? 'закрыт' : 'открыт';
                const text = val ? t('table.statusClosed') : t('table.statusOpen');
                return `<span class="status-badge ${status}">${text}</span>`;
            }
        },
        { label: t('table.operations'), field: 'количествоОпераций' }
    ];
    
    const tableHtml = createTable(columns, mlList, {
        emptyMessage: t('message.noMl'),
        onRowClick: (index) => selectMl(mlList[index])
    });
    
    container.innerHTML = tableHtml;
    
    // Добавляем обработчики кликов на строки
    const rows = container.querySelectorAll('tbody tr');
    rows.forEach((row, index) => {
        row.classList.add('table-row-clickable');
        row.addEventListener('click', () => selectMl(mlList[index]));
    });
    
    // Добавляем пагинацию
    if (totalPages > 1) {
        const paginationHtml = createMlPagination();
        container.insertAdjacentHTML('afterend', paginationHtml);
    }
    
    section.classList.remove('hidden');
}

// Создание пагинации для МЛ
function createMlPagination() {
    let html = '<div class="pagination" style="margin-top: var(--spacing-md);">';
    
    // Кнопка "Назад"
    const prevDisabled = currentPage === 1 ? 'disabled' : '';
    html += `<button class="pagination-btn" onclick="changePage(${currentPage - 1})" ${prevDisabled}>${t('pagination.prev')}</button>`;
    
    // Номера страниц
    const maxButtons = 5;
    let startPage = Math.max(1, currentPage - Math.floor(maxButtons / 2));
    let endPage = Math.min(totalPages, startPage + maxButtons - 1);
    
    if (endPage - startPage < maxButtons - 1) {
        startPage = Math.max(1, endPage - maxButtons + 1);
    }
    
    if (startPage > 1) {
        html += `<button class="pagination-btn" onclick="changePage(1)">1</button>`;
        if (startPage > 2) {
            html += '<span class="pagination-info">...</span>';
        }
    }
    
    for (let i = startPage; i <= endPage; i++) {
        const activeClass = i === currentPage ? 'active' : '';
        html += `<button class="pagination-btn ${activeClass}" onclick="changePage(${i})">${i}</button>`;
    }
    
    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            html += '<span class="pagination-info">...</span>';
        }
        html += `<button class="pagination-btn" onclick="changePage(${totalPages})">${totalPages}</button>`;
    }
    
    // Кнопка "Вперед"
    const nextDisabled = currentPage === totalPages ? 'disabled' : '';
    html += `<button class="pagination-btn" onclick="changePage(${currentPage + 1})" ${nextDisabled}>${t('pagination.next')}</button>`;
    
    // Селектор размера страницы
    html += `
        <div class="pagination-size-selector" style="margin-left: auto;">
            <label for="mlPageSize">${t('pagination.recordsPerPage')}:</label>
            <select id="mlPageSize" onchange="changePageSize(parseInt(this.value))">
                <option value="10" ${pageSize === 10 ? 'selected' : ''}>10</option>
                <option value="20" ${pageSize === 20 ? 'selected' : ''}>20</option>
                <option value="50" ${pageSize === 50 ? 'selected' : ''}>50</option>
                <option value="100" ${pageSize === 100 ? 'selected' : ''}>100</option>
            </select>
        </div>
    `;
    
    html += '</div>';
    return html;
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
        
        // Скрыть блок поиска
        document.querySelector('.search-bar').classList.add('hidden');
        
        // Прокрутка к деталям
        document.getElementById('mlDetailsSection').scrollIntoView({ behavior: 'smooth' });
        
    } catch (error) {
        console.error('Ошибка загрузки операций:', error);
        showToast(t('message.errorLoadingOperations'), 'error');
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
                <h3>${t('message.noOperations')}</h3>
                <p>${t('message.noOperationsForMl')}</p>
            </div>
        `;
        return;
    }
    
    const columns = [
        { label: t('table.operationType'), field: 'типОперации' },
        { 
            label: t('table.employee'), 
            field: 'сотрудник',
            format: (val, row) => formatEmployee(row.табельныйНомер, val)
        },
        { label: t('table.quantity'), field: 'количество' },
        { label: t('table.timeStandard'), field: 'нормаВремениЧас', format: (val) => formatNumber(val, 2) },
        { label: t('table.tariff'), field: 'названиеТарифа' },
        { label: t('table.pricePerHour'), field: 'ценаЗаЧас', format: (val) => formatNumber(val, 2) },
        { label: t('table.executionDate'), field: 'датаИсполнения', format: (val) => formatDateTime(val) },
        { label: t('table.closureDate'), field: 'датаЗакрытия', format: (val) => formatDateTime(val) },
        { label: t('table.baseSum'), field: 'базоваяСумма', format: (val) => formatNumber(val, 2) },
        { label: t('table.dealCoef'), field: 'коэффициентСделки', format: (val) => formatNumber(val, 2) },
        { label: t('table.surcharge'), field: 'суммаНадбавки', format: (val) => formatNumber(val, 2) },
        { label: t('table.premiumCoef'), field: 'коэффициентПремии', format: (val) => formatNumber(val, 2) },
        { label: t('table.premium'), field: 'суммаПремии', format: (val) => formatNumber(val, 2) },
        { label: t('table.total'), field: 'итого', format: (val) => formatNumber(val, 2) },
        { 
            label: t('table.status'), 
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
            label: '📊',
            class: 'btn-sm btn-info',
            title: t('action.showCalculation'),
            handler: (index) => showCalculationDetails(operations[index]),
            disabled: () => false
        },
        {
            label: t('action.finish'),
            class: 'btn-sm btn-success',
            handler: (index) => finishOperation(operations[index]),
            disabled: (row) => row.статус === 'Завершена' || row.статус === 'Отменена'
        },
        {
            label: t('action.cancel'),
            class: 'btn-sm btn-warning',
            handler: (index) => cancelOperation(operations[index]),
            disabled: (row) => row.статус !== 'Завершена'
        }
    ];
    
    const tableHtml = createTable(columns, operations, {
        emptyMessage: t('message.noOperations'),
        rowActions: rowActions
    });
    
    container.innerHTML = tableHtml;
    
    // Добавляем обработчики событий для кнопок действий
    const actionButtons = container.querySelectorAll('.table-actions button');
    actionButtons.forEach((button, btnIndex) => {
        const rowIndex = Math.floor(btnIndex / rowActions.length);
        const actionIndex = btnIndex % rowActions.length;
        const action = rowActions[actionIndex];
        
        button.addEventListener('click', (e) => {
            e.stopPropagation();
            if (!button.disabled) {
                action.handler(rowIndex);
            }
        });
    });
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
        `${t('confirm.finishOperation')} "${operation.типОперации}"?`,
        async () => {
            try {
                await operationApi.finish(operation.id);
                showToast(t('message.operationFinished'), 'success');
                
                // Перезагрузка операций
                await selectMl(currentMl);
                
            } catch (error) {
                console.error('Ошибка завершения операции:', error);
                showToast(t('message.errorFinishOperation'), 'error');
            }
        }
    );
}

// Отмена операции
async function cancelOperation(operation) {
    const message = `${t('confirm.cancelOperation')} "${operation.типОперации}"?\n\n${t('confirm.cancelOperationText')}`;
    
    confirmDialog(
        message,
        async () => {
            try {
                await operationApi.cancel(operation.id);
                showToast(t('message.operationCancelled'), 'success');
                
                // Перезагрузка операций
                await selectMl(currentMl);
                
            } catch (error) {
                console.error('Ошибка отмены операции:', error);
                const message = error.response?.data?.message || t('message.errorCancelOperation');
                showToast(message, 'error');
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
        showToast(t('message.selectEmployee'), 'warning');
        return;
    }
    
    const data = {
        сотрудникОТКID: employeeId,
        количествоОТК: quantity,
        количествоБрак: defects
    };
    
    try {
        await mlApi.close(currentMl.id, data);
        showToast(t('message.mlClosed'), 'success');
        
        hideModal('closeMlModal');
        
        // Обновление данных
        currentMl.закрыт = true;
        displayMlDetails(currentMl);
        
        // Очистка формы
        document.getElementById('closeMlForm').reset();
        
    } catch (error) {
        console.error('Ошибка закрытия МЛ:', error);
        showToast(error.message || t('message.errorCloseMl'), 'error');
    }
}

// Возврат к списку МЛ
function backToList() {
    document.getElementById('mlDetailsSection').classList.add('hidden');
    document.getElementById('mlListSection').classList.remove('hidden');
    
    // Показать блок поиска
    document.querySelector('.search-bar').classList.remove('hidden');
    
    // Сброс фильтров операций
    document.getElementById('operationStatusFilter').value = '';
    document.getElementById('operationEmployeeFilter').value = '';
    document.getElementById('operationDateFrom').value = '';
    document.getElementById('operationDateTo').value = '';
    
    currentMl = null;
    currentOperations = [];
    filteredOperations = [];
}

// Показать детали расчета операции
async function showCalculationDetails(operation) {
    try {
        const calculation = await operationApi.getCalculation(operation.id);
        
        const content = document.getElementById('calculationContent');
        
        let html = '<div class="calculation-details">';
        
        // Входные данные
        html += '<div class="calculation-section">';
        html += `<h3>${t('modal.calculation.inputData')}</h3>`;
        html += '<div class="calculation-grid">';
        html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.quantity')}:</span><span class="calc-value">${calculation.входныеДанные.количество || 0}</span></div>`;
        html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.timeStandard')}:</span><span class="calc-value">${formatNumber(calculation.входныеДанные.нормаВремениЧас, 2)}</span></div>`;
        html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.pricePerHour')}:</span><span class="calc-value">${formatNumber(calculation.входныеДанные.ценаЗаЧас, 2)}</span></div>`;
        html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.dealCoef')}:</span><span class="calc-value">${formatNumber(calculation.входныеДанные.коэффициентСделки, 2)}</span></div>`;
        html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.executionDate')}:</span><span class="calc-value">${formatDateTime(calculation.входныеДанные.датаИсполнения)}</span></div>`;
        html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.closureDate')}:</span><span class="calc-value">${formatDateTime(calculation.входныеДанные.датаЗакрытия)}</span></div>`;
        html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.onTime')}:</span><span class="calc-value">${calculation.входныеДанные.вСрок ? t('modal.calculation.yes') + ' ✓' : t('modal.calculation.no') + ' ✗'}</span></div>`;
        html += '</div>';
        html += '</div>';
        
        // Источник коэффициента сделки
        if (calculation.источникКоэффициентаСделки) {
            html += '<div class="calculation-section">';
            html += `<h3>${t('modal.calculation.coefSource')}</h3>`;
            
            if (calculation.источникКоэффициентаСделки.найден) {
                html += '<div class="calculation-grid">';
                html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.product')}:</span><span class="calc-value">${calculation.источникКоэффициентаСделки.изделие || '-'}</span></div>`;
                html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.dce')}:</span><span class="calc-value">${calculation.источникКоэффициентаСделки.дсе || '-'}</span></div>`;
                html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.operationType')}:</span><span class="calc-value">${calculation.источникКоэффициентаСделки.типОперации || '-'}</span></div>`;
                html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.coefficient')}:</span><span class="calc-value">${formatNumber(calculation.источникКоэффициентаСделки.коэффициент, 2)}</span></div>`;
                html += `<div class="calc-item"><span class="calc-label">${t('modal.calculation.validPeriod')}:</span><span class="calc-value">${formatDate(calculation.источникКоэффициентаСделки.датаНачала)} - ${formatDate(calculation.источникКоэффициентаСделки.датаОкончания) || t('modal.calculation.indefinite')}</span></div>`;
                html += '</div>';
            } else {
                html += `<p class="text-muted">${t('modal.calculation.notFound')}</p>`;
            }
            
            html += '</div>';
        }
        
        // Формулы и расчеты
        html += '<div class="calculation-section">';
        html += `<h3>${t('modal.calculation.formulas')}</h3>`;
        
        // 1. Базовая сумма
        html += '<div class="formula-block">';
        html += `<div class="formula-title">1. ${t('modal.calculation.baseSum')}</div>`;
        html += `<div class="formula-text">${t('modal.calculation.baseSumFormula')}</div>`;
        html += `<div class="formula-calc">${calculation.входныеДанные.количество} × ${formatNumber(calculation.входныеДанные.нормаВремениЧас, 2)} × ${formatNumber(calculation.входныеДанные.ценаЗаЧас, 2)} = <strong>${formatNumber(calculation.результаты.базоваяСумма, 2)} ₽</strong></div>`;
        html += '</div>';
        
        // 2. Сумма надбавки
        html += '<div class="formula-block">';
        html += `<div class="formula-title">2. ${t('modal.calculation.surcharge')}</div>`;
        html += `<div class="formula-text">${t('modal.calculation.surchargeFormula')}</div>`;
        html += `<div class="formula-calc">(${formatNumber(calculation.входныеДанные.коэффициентСделки, 2)} - 1) × ${formatNumber(calculation.результаты.базоваяСумма, 2)} = <strong>${formatNumber(calculation.результаты.суммаНадбавки, 2)} ₽</strong></div>`;
        html += '</div>';
        
        // 3. Коэффициент премии
        html += '<div class="formula-block">';
        html += `<div class="formula-title">3. ${t('modal.calculation.premiumCoef')}</div>`;
        html += `<div class="formula-text">${t('modal.calculation.premiumCoefFormula')}</div>`;
        const onTimeText = calculation.входныеДанные.вСрок ? t('modal.calculation.onTimeText') : t('modal.calculation.lateText');
        html += `<div class="formula-calc">${t('message.operationFinished').split(' ')[0]} <strong>${onTimeText}</strong> → ${t('modal.calculation.coefficient')} = <strong>${formatNumber(calculation.результаты.коэффициентПремии, 2)}</strong></div>`;
        html += '</div>';
        
        // 4. Сумма премии
        html += '<div class="formula-block">';
        html += `<div class="formula-title">4. ${t('modal.calculation.premium')}</div>`;
        html += `<div class="formula-text">${t('modal.calculation.premiumFormula')}</div>`;
        html += `<div class="formula-calc">${formatNumber(calculation.результаты.коэффициентПремии, 2)} × (${formatNumber(calculation.результаты.суммаНадбавки, 2)} + ${formatNumber(calculation.результаты.базоваяСумма, 2)}) = <strong>${formatNumber(calculation.результаты.суммаПремии, 2)} ₽</strong></div>`;
        html += '</div>';
        
        // 5. Итого
        html += '<div class="formula-block formula-total">';
        html += `<div class="formula-title">5. ${t('modal.calculation.totalSum')}</div>`;
        html += `<div class="formula-text">${t('modal.calculation.totalFormula')}</div>`;
        html += `<div class="formula-calc">${formatNumber(calculation.результаты.базоваяСумма, 2)} + ${formatNumber(calculation.результаты.суммаНадбавки, 2)} + ${formatNumber(calculation.результаты.суммаПремии, 2)} = <strong>${formatNumber(calculation.результаты.итого, 2)} ₽</strong></div>`;
        html += '</div>';
        
        html += '</div>';
        html += '</div>';
        
        content.innerHTML = html;
        showModal('calculationModal');
        
    } catch (error) {
        console.error('Ошибка загрузки расчета:', error);
        showToast(t('message.errorLoadingCalculation'), 'error');
    }
}

// Экспорт функций в глобальную область для использования в HTML
window.changePage = changePage;
window.changePageSize = changePageSize;
window.finishOperation = finishOperation;

// Обработчик смены языка
document.addEventListener('languageChanged', () => {
    // Перезаполнить селекты с переводами
    populateProductSelect();
    populateEmployeeSelects();
    
    // Перезагрузить текущие данные если они есть
    if (currentMl) {
        displayMlDetails(currentMl);
        displayOperations(filteredOperations);
    } else if (currentMlList.length > 0) {
        displayMlList(currentMlList);
    }
});

// ========================================
// ЭКСПОРТ
// ========================================

// Получить текущие фильтры поиска для экспорта
function getCurrentSearchFilters() {
    const filters = {};
    const searchType = document.getElementById('searchType').value;
    const status = document.getElementById('statusFilter').value;
    
    // Номер МЛ
    if (searchType === 'ml') {
        const mlNumber = document.getElementById('mlNumber').value.trim();
        if (mlNumber) {
            filters.номерМЛ = mlNumber;
        }
    }
    
    // Изделие
    if (searchType === 'product' || searchType === 'dce') {
        const productId = document.getElementById('productSelect').value;
        if (productId) {
            filters.изделиеId = parseInt(productId);
        }
    }
    
    // ДСЕ
    if (searchType === 'dce') {
        const dceId = document.getElementById('dceSelect').value;
        if (dceId) {
            filters.дсеId = parseInt(dceId);
        }
    }
    
    // Статус
    if (status === 'open') {
        filters.закрыт = false;
    } else if (status === 'closed') {
        filters.закрыт = true;
    }
    
    return filters;
}
