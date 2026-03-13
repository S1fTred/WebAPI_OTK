// ========================================
// МОДУЛЬ КАТАЛОГА ДСЕ
// ========================================

// Глобальное состояние
let currentPage = 1;
let pageSize = 10; // Уменьшено для демонстрации пагинации
let totalRecords = 0;
let totalPages = 0;
let products = [];
let currentFilters = {
    поиск: '',
    изделиеID: null
};

// ========================================
// ИНИЦИАЛИЗАЦИЯ
// ========================================

document.addEventListener('DOMContentLoaded', async () => {
    // Проверка авторизации
    if (!checkAuth()) return;
    
    // Загрузка справочников
    await loadReferenceData();
    
    // Загрузка ДСЕ
    await loadDCE();
    
    // Инициализация обработчиков
    initEventHandlers();
    
    // Обработчик выхода
    document.getElementById('logoutBtn').addEventListener('click', logout);
});

// Загрузка справочных данных
async function loadReferenceData() {
    try {
        // Загрузка изделий
        products = await productApi.getAll();
        
        // Заполнение селекта изделий
        const productFilter = document.getElementById('productFilter');
        productFilter.innerHTML = '<option value="">Все изделия</option>';
        
        products.forEach(product => {
            const option = document.createElement('option');
            option.value = product.id;
            option.textContent = product.наименование;
            productFilter.appendChild(option);
        });
        
    } catch (error) {
        console.error('Ошибка загрузки справочников:', error);
        showToast('Ошибка загрузки справочных данных', 'error');
    }
}

// ========================================
// ОБРАБОТЧИКИ СОБЫТИЙ
// ========================================

function initEventHandlers() {
    // Поиск
    const searchInput = document.getElementById('searchInput');
    searchInput.addEventListener('input', debounce(() => {
        currentPage = 1;
        applyFilters();
    }, 300));
    
    // Фильтр по изделию
    document.getElementById('productFilter').addEventListener('change', () => {
        currentPage = 1;
        applyFilters();
    });
    
    // Изменение размера страницы
    document.getElementById('pageSizeSelect').addEventListener('change', (e) => {
        pageSize = parseInt(e.target.value);
        currentPage = 1;
        loadDCE();
    });
    
    // Сброс фильтров
    document.getElementById('resetFiltersBtn').addEventListener('click', resetFilters);
    
    // Закрытие модального окна
    document.querySelector('#dceModal .modal-close').addEventListener('click', () => {
        hideModal('dceModal');
    });
}

// ========================================
// ЗАГРУЗКА И ОТОБРАЖЕНИЕ ДСЕ
// ========================================

async function loadDCE() {
    try {
        const params = {
            page: currentPage,
            pageSize: pageSize
        };
        
        if (currentFilters.поиск) {
            params.search = currentFilters.поиск;
        }
        
        if (currentFilters.изделиеID) {
            params.изделиеId = currentFilters.изделиеID;
        }
        
        const response = await dceApi.getAll(params);
        
        totalRecords = response.всегоЗаписей;
        totalPages = response.всегоСтраниц;
        
        displayDCE(response.данные);
        updatePaginationInfo();
        renderPagination();
        
    } catch (error) {
        console.error('Ошибка загрузки ДСЕ:', error);
        showToast('Ошибка загрузки каталога ДСЕ', 'error');
    }
}

// Применение фильтров
function applyFilters() {
    const searchTerm = document.getElementById('searchInput').value.trim();
    const productId = document.getElementById('productFilter').value;
    
    currentFilters.поиск = searchTerm;
    currentFilters.изделиеID = productId ? parseInt(productId) : null;
    
    loadDCE();
}

// Сброс фильтров
function resetFilters() {
    document.getElementById('searchInput').value = '';
    document.getElementById('productFilter').value = '';
    
    currentPage = 1;
    currentFilters = {
        поиск: '',
        изделиеID: null
    };
    
    loadDCE();
}

// Отображение ДСЕ в таблице
function displayDCE(dceList) {
    const tbody = document.getElementById('dceTableBody');
    
    if (!dceList || dceList.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="4" class="empty-state">
                    <div class="empty-state-icon">📦</div>
                    <h3>ДСЕ не найдены</h3>
                    <p>Попробуйте изменить параметры поиска</p>
                </td>
            </tr>
        `;
        return;
    }
    
    let html = '';
    
    dceList.forEach(dce => {
        html += `
            <tr>
                <td><strong>${escapeHtml(dce.код)}</strong></td>
                <td>${escapeHtml(dce.наименование || '-')}</td>
                <td>${escapeHtml(dce.изделие || '-')}</td>
                <td>
                    <button class="btn btn-sm btn-primary" onclick="showDCEDetails(${dce.id})">
                        👁️ Подробнее
                    </button>
                </td>
            </tr>
        `;
    });
    
    tbody.innerHTML = html;
}

// Обновление информации о пагинации
function updatePaginationInfo() {
    const recordsInfo = document.getElementById('recordsInfo');
    const start = (currentPage - 1) * pageSize + 1;
    const end = Math.min(currentPage * pageSize, totalRecords);
    
    recordsInfo.textContent = `Показано ${start}-${end} из ${totalRecords} записей`;
}

// Отрисовка пагинации
function renderPagination() {
    const container = document.getElementById('paginationContainer');
    
    if (totalPages <= 1) {
        container.innerHTML = '';
        return;
    }
    
    let html = '';
    
    // Кнопка "Предыдущая"
    html += `
        <button class="pagination-btn" ${currentPage === 1 ? 'disabled' : ''} 
                onclick="changePage(${currentPage - 1})">
            ← Предыдущая
        </button>
    `;
    
    // Номера страниц
    const maxButtons = 7;
    let startPage = Math.max(1, currentPage - Math.floor(maxButtons / 2));
    let endPage = Math.min(totalPages, startPage + maxButtons - 1);
    
    if (endPage - startPage < maxButtons - 1) {
        startPage = Math.max(1, endPage - maxButtons + 1);
    }
    
    if (startPage > 1) {
        html += `<button class="pagination-btn" onclick="changePage(1)">1</button>`;
        if (startPage > 2) {
            html += `<span class="pagination-ellipsis">...</span>`;
        }
    }
    
    for (let i = startPage; i <= endPage; i++) {
        html += `
            <button class="pagination-btn ${i === currentPage ? 'active' : ''}" 
                    onclick="changePage(${i})">
                ${i}
            </button>
        `;
    }
    
    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            html += `<span class="pagination-ellipsis">...</span>`;
        }
        html += `<button class="pagination-btn" onclick="changePage(${totalPages})">${totalPages}</button>`;
    }
    
    // Кнопка "Следующая"
    html += `
        <button class="pagination-btn" ${currentPage === totalPages ? 'disabled' : ''} 
                onclick="changePage(${currentPage + 1})">
            Следующая →
        </button>
    `;
    
    container.innerHTML = html;
}

// Изменение страницы
function changePage(page) {
    if (page < 1 || page > totalPages || page === currentPage) return;
    
    currentPage = page;
    loadDCE();
    
    // Прокрутка к началу таблицы
    document.querySelector('.table-container').scrollIntoView({ behavior: 'smooth' });
}

// ========================================
// ДЕТАЛИ ДСЕ
// ========================================

async function showDCEDetails(id) {
    try {
        const dce = await dceApi.getById(id);
        
        const detailsHtml = `
            <div class="info-grid">
                <div class="info-item">
                    <span class="info-label">Код:</span>
                    <span class="info-value"><strong>${escapeHtml(dce.код)}</strong></span>
                </div>
                <div class="info-item">
                    <span class="info-label">Наименование:</span>
                    <span class="info-value">${escapeHtml(dce.наименование || '-')}</span>
                </div>
                <div class="info-item">
                    <span class="info-label">Изделие:</span>
                    <span class="info-value">${escapeHtml(dce.изделие || '-')}</span>
                </div>
                <div class="info-item">
                    <span class="info-label">ID:</span>
                    <span class="info-value">${dce.id}</span>
                </div>
            </div>
        `;
        
        document.getElementById('dceDetails').innerHTML = detailsHtml;
        showModal('dceModal');
        
    } catch (error) {
        console.error('Ошибка загрузки деталей ДСЕ:', error);
        showToast('Ошибка загрузки деталей', 'error');
    }
}

// Экспорт функций в глобальную область для использования в HTML
window.changePage = changePage;
window.showDCEDetails = showDCEDetails;
