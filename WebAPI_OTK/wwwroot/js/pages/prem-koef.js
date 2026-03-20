// ========================================
// МОДУЛЬ ПРЕМИАЛЬНЫХ КОЭФФИЦИЕНТОВ
// ========================================

// Глобальное состояние
let allKoefs = [];
let filteredKoefs = [];
let products = [];
let dceList = [];
let operationTypes = [];
let editingKoef = null;

// ========================================
// ИНИЦИАЛИЗАЦИЯ
// ========================================

document.addEventListener('DOMContentLoaded', async () => {
    // Проверка авторизации
    if (!checkAuth()) return;
    
    // Загрузка справочников
    await loadReferenceData();
    
    // Загрузка коэффициентов
    await loadKoefs();
    
    // Инициализация обработчиков
    initEventHandlers();
    
    // Обработчик выхода
    document.getElementById('logoutBtn').addEventListener('click', logout);
});

// Загрузка справочных данных
async function loadReferenceData() {
    try {
        // Загрузка изделий
        try {
            products = await productApi.getAll();
        } catch (error) {
            console.warn('Не удалось загрузить изделия:', error);
            products = [];
        }
        
        // Загрузка типов операций
        try {
            operationTypes = await operationTypeApi.getAll();
        } catch (error) {
            console.warn('Не удалось загрузить типы операций:', error);
            operationTypes = [];
        }
        
        // Заполнение селектов
        populateSelects();
        
    } catch (error) {
        console.error('Критическая ошибка загрузки справочников:', error);
        showToast(t('message.serverError'), 'error');
    }
}

// Заполнение селектов
function populateSelects() {
    // Фильтр изделий
    const productFilter = document.getElementById('productFilter');
    productFilter.innerHTML = `<option value="">${t('premKoef.allProducts')}</option>`;
    
    // Форма - изделие
    const koefProduct = document.getElementById('koefProduct');
    koefProduct.innerHTML = `<option value="">${t('modal.koef.productNotSpecified')}</option>`;
    
    products.forEach(product => {
        const option1 = document.createElement('option');
        option1.value = product.id;
        option1.textContent = product.наименование;
        productFilter.appendChild(option1);
        
        const option2 = document.createElement('option');
        option2.value = product.id;
        option2.textContent = product.наименование;
        koefProduct.appendChild(option2);
    });
    
    // Фильтр типов операций
    const operationTypeFilter = document.getElementById('operationTypeFilter');
    operationTypeFilter.innerHTML = `<option value="">${t('premKoef.allTypes')}</option>`;
    
    // Форма - тип операции
    const koefOperationType = document.getElementById('koefOperationType');
    koefOperationType.innerHTML = `<option value="">${t('modal.koef.operationTypeNotSpecified')}</option>`;
    
    operationTypes.forEach(type => {
        const option1 = document.createElement('option');
        option1.value = type.id;
        option1.textContent = type.наименование;
        operationTypeFilter.appendChild(option1);
        
        const option2 = document.createElement('option');
        option2.value = type.id;
        option2.textContent = type.наименование;
        koefOperationType.appendChild(option2);
    });
}

// ========================================
// ОБРАБОТЧИКИ СОБЫТИЙ
// ========================================

function initEventHandlers() {
    // Кнопка добавления
    document.getElementById('addKoefBtn').addEventListener('click', () => openKoefModal());
    
    // Поиск
    const searchInput = document.getElementById('searchInput');
    searchInput.addEventListener('input', debounce(() => applyFilters(), 300));
    
    // Фильтры
    document.getElementById('statusFilter').addEventListener('change', applyFilters);
    document.getElementById('productFilter').addEventListener('change', applyFilters);
    document.getElementById('operationTypeFilter').addEventListener('change', applyFilters);
    document.getElementById('dateFilter').addEventListener('change', applyFilters);
    
    // Сброс фильтров
    document.getElementById('resetFiltersBtn').addEventListener('click', resetFilters);
    
    // Форма коэффициента
    document.getElementById('koefForm').addEventListener('submit', handleSaveKoef);
    
    // Изменение изделия в форме - загрузка ДСЕ
    document.getElementById('koefProduct').addEventListener('change', handleProductChangeInForm);
    
    // Закрытие модального окна
    document.querySelector('#koefModal .modal-close').addEventListener('click', () => {
        hideModal('koefModal');
    });
}

// Обработка изменения изделия в форме
async function handleProductChangeInForm(e) {
    const productId = e.target.value;
    const dceSelect = document.getElementById('koefDce');
    
    dceSelect.innerHTML = `<option value="">${t('modal.koef.dceNotSpecified')}</option>`;
    
    if (!productId) return;
    
    try {
        const dceData = await productApi.getDCE(productId);
        dceData.forEach(dce => {
            const option = document.createElement('option');
            option.value = dce.id;
            option.textContent = `${dce.код} - ${dce.наименование || ''}`;
            dceSelect.appendChild(option);
        });
    } catch (error) {
        console.error('Ошибка загрузки ДСЕ:', error);
    }
}

// ========================================
// ЗАГРУЗКА И ОТОБРАЖЕНИЕ КОЭФФИЦИЕНТОВ
// ========================================

async function loadKoefs() {
    try {
        const koefs = await premKoefApi.getAll({ толькоАктивные: false });
        allKoefs = koefs;
        applyFilters();
        updateStatistics();
    } catch (error) {
        console.error('Ошибка загрузки коэффициентов:', error);
        showToast(t('message.errorLoadingKoefs'), 'error');
    }
}

// Применение фильтров
function applyFilters() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    const statusFilter = document.getElementById('statusFilter').value;
    const productFilter = document.getElementById('productFilter').value;
    const operationTypeFilter = document.getElementById('operationTypeFilter').value;
    const dateFilter = document.getElementById('dateFilter').value;
    
    let filtered = [...allKoefs];
    
    // Поиск по наименованию
    if (searchTerm) {
        filtered = filtered.filter(k => 
            k.наименование.toLowerCase().includes(searchTerm)
        );
    }
    
    // Фильтр по статусу
    if (statusFilter === 'active') {
        filtered = filtered.filter(k => k.активный === true);
    } else if (statusFilter === 'inactive') {
        filtered = filtered.filter(k => k.активный === false);
    }
    
    // Фильтр по изделию
    if (productFilter) {
        filtered = filtered.filter(k => k.изделиеID === parseInt(productFilter));
    }
    
    // Фильтр по типу операции
    if (operationTypeFilter) {
        filtered = filtered.filter(k => k.типОперацииID === parseInt(operationTypeFilter));
    }
    
    // Фильтр по дате
    if (dateFilter) {
        const checkDate = new Date(dateFilter);
        filtered = filtered.filter(k => {
            const startDate = new Date(k.датаНачала);
            const endDate = k.датаОкончания ? new Date(k.датаОкончания) : null;
            
            return startDate <= checkDate && (!endDate || endDate >= checkDate);
        });
    }
    
    filteredKoefs = filtered;
    displayKoefs(filtered);
}

// Сброс фильтров
function resetFilters() {
    document.getElementById('searchInput').value = '';
    document.getElementById('statusFilter').value = 'active';
    document.getElementById('productFilter').value = '';
    document.getElementById('operationTypeFilter').value = '';
    document.getElementById('dateFilter').value = '';
    
    applyFilters();
}

// Отображение коэффициентов
function displayKoefs(koefs) {
    const container = document.getElementById('koefListContainer');
    
    if (koefs.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <div class="empty-state-icon">💰</div>
                <h3>${t('premKoef.notFound')}</h3>
                <p>${t('premKoef.tryChangeFilters')}</p>
            </div>
        `;
        return;
    }
    
    let html = '';
    
    koefs.forEach(koef => {
        const isActive = koef.активный;
        const cardClass = isActive ? '' : 'inactive';
        const statusBadge = isActive 
            ? `<span class="badge badge-success">${t('premKoef.statusActive')}</span>`
            : `<span class="badge badge-error">${t('premKoef.statusInactive')}</span>`;
        
        html += `
            <div class="koef-card ${cardClass}">
                <div class="koef-card-header">
                    <div>
                        <div class="koef-card-title">${escapeHtml(koef.наименование)}</div>
                        ${statusBadge}
                    </div>
                    <div class="koef-card-value">×${formatNumber(koef.коэффициент, 2)}</div>
                </div>
                
                <div class="koef-card-body">
                    <div class="info-item">
                        <span class="info-label">${t('premKoef.validPeriod')}:</span>
                        <span class="info-value">
                            ${formatDate(koef.датаНачала)} - ${koef.датаОкончания ? formatDate(koef.датаОкончания) : t('premKoef.indefinite')}
                        </span>
                    </div>
                    <div class="info-item">
                        <span class="info-label">${t('premKoef.product')}:</span>
                        <span class="info-value">${koef.изделие || t('premKoef.all')}</span>
                    </div>
                    <div class="info-item">
                        <span class="info-label">${t('premKoef.dce')}:</span>
                        <span class="info-value">${koef.дсе || t('premKoef.all')}</span>
                    </div>
                    <div class="info-item">
                        <span class="info-label">${t('premKoef.operationType')}:</span>
                        <span class="info-value">${koef.типОперации || t('premKoef.all')}</span>
                    </div>
                </div>
                
                <div class="koef-card-footer">
                    <div class="record-actions">
                        <button class="btn btn-sm btn-primary" onclick="editKoef(${koef.id})">
                            ${t('premKoef.edit')}
                        </button>
                        ${isActive ? `
                            <button class="btn btn-sm btn-warning" onclick="deactivateKoef(${koef.id})">
                                ${t('premKoef.deactivate')}
                            </button>
                        ` : `
                            <button class="btn btn-sm btn-danger" onclick="deleteKoef(${koef.id})">
                                ${t('premKoef.delete')}
                            </button>
                        `}
                    </div>
                </div>
            </div>
        `;
    });
    
    container.innerHTML = html;
}

// Обновление статистики
function updateStatistics() {
    const total = allKoefs.length;
    const active = allKoefs.filter(k => k.активный === true).length;
    const inactive = total - active;
    
    document.getElementById('totalKoefCount').textContent = total;
    document.getElementById('activeKoefCount').textContent = active;
    document.getElementById('inactiveKoefCount').textContent = inactive;
}

// ========================================
// CRUD ОПЕРАЦИИ
// ========================================

// Открытие модального окна для создания/редактирования
function openKoefModal(koef = null) {
    editingKoef = koef;
    
    const title = document.getElementById('modalTitle');
    const form = document.getElementById('koefForm');
    
    if (koef) {
        // Редактирование
        title.textContent = t('modal.koef.titleEdit');
        title.setAttribute('data-i18n', 'modal.koef.titleEdit');
        document.getElementById('koefId').value = koef.id;
        document.getElementById('koefName').value = koef.наименование;
        document.getElementById('koefValue').value = koef.коэффициент;
        document.getElementById('koefStartDate').value = koef.датаНачала.split('T')[0];
        document.getElementById('koefEndDate').value = koef.датаОкончания ? koef.датаОкончания.split('T')[0] : '';
        document.getElementById('koefProduct').value = koef.изделиеID || '';
        document.getElementById('koefDce').value = koef.дсеID || '';
        document.getElementById('koefOperationType').value = koef.типОперацииID || '';
        
        // Загрузка ДСЕ если выбрано изделие
        if (koef.изделиеID) {
            handleProductChangeInForm({ target: { value: koef.изделиеID } })
                .then(() => {
                    document.getElementById('koefDce').value = koef.дсеID || '';
                });
        }
    } else {
        // Создание
        title.textContent = t('modal.koef.titleAdd');
        title.setAttribute('data-i18n', 'modal.koef.titleAdd');
        form.reset();
        document.getElementById('koefId').value = '';
        // Установка текущей даты по умолчанию
        document.getElementById('koefStartDate').value = getCurrentDate();
    }
    
    showModal('koefModal');
}

// Редактирование коэффициента
async function editKoef(id) {
    try {
        const koef = await premKoefApi.getById(id);
        openKoefModal(koef);
    } catch (error) {
        console.error('Ошибка загрузки коэффициента:', error);
        showToast(t('message.errorLoadingKoef'), 'error');
    }
}

// Сохранение коэффициента
async function handleSaveKoef(e) {
    e.preventDefault();
    
    const id = document.getElementById('koefId').value;
    const data = {
        наименование: document.getElementById('koefName').value,
        коэффициент: parseFloat(document.getElementById('koefValue').value),
        датаНачала: document.getElementById('koefStartDate').value,
        датаОкончания: document.getElementById('koefEndDate').value || null,
        изделиеID: document.getElementById('koefProduct').value ? parseInt(document.getElementById('koefProduct').value) : null,
        дсеID: document.getElementById('koefDce').value ? parseInt(document.getElementById('koefDce').value) : null,
        типОперацииID: document.getElementById('koefOperationType').value ? parseInt(document.getElementById('koefOperationType').value) : null
    };
    
    try {
        if (id) {
            // Обновление
            await premKoefApi.update(parseInt(id), data);
            showToast(t('message.koefUpdated'), 'success');
        } else {
            // Создание
            await premKoefApi.create(data);
            showToast(t('message.koefCreated'), 'success');
        }
        
        hideModal('koefModal');
        await loadKoefs();
        
    } catch (error) {
        console.error('Ошибка сохранения коэффициента:', error);
        showToast(error.message || t('message.errorSavingKoef'), 'error');
    }
}

// Деактивация коэффициента
async function deactivateKoef(id) {
    confirmDialog(
        t('confirm.deactivateKoef'),
        async () => {
            try {
                await premKoefApi.deactivate(id);
                showToast(t('message.koefDeactivated'), 'success');
                await loadKoefs();
            } catch (error) {
                console.error('Ошибка деактивации коэффициента:', error);
                showToast(error.message || t('message.errorDeactivatingKoef'), 'error');
            }
        }
    );
}

// Удаление коэффициента
async function deleteKoef(id) {
    confirmDialog(
        t('confirm.deleteKoef'),
        async () => {
            try {
                await premKoefApi.delete(id);
                showToast(t('message.koefDeleted'), 'success');
                await loadKoefs();
            } catch (error) {
                console.error('Ошибка удаления коэффициента:', error);
                showToast(error.message || t('message.errorDeletingKoef'), 'error');
            }
        }
    );
}

// Экспорт функций в глобальную область для использования в HTML
window.editKoef = editKoef;
window.deactivateKoef = deactivateKoef;
window.deleteKoef = deleteKoef;

// Обработчик смены языка
document.addEventListener('languageChanged', () => {
    populateSelects();
    displayKoefs(filteredKoefs);
    updateStatistics();
});
