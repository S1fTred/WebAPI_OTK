// ========================================
// ПЕРЕИСПОЛЬЗУЕМЫЕ КОМПОНЕНТЫ
// ========================================

// Контейнер для toast уведомлений
let toastContainer = null;

// Инициализация контейнера для toast
function initToastContainer() {
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.className = 'toast-container';
        document.body.appendChild(toastContainer);
    }
}

// Показать toast уведомление
function showToast(message, type = 'info', duration = 3000) {
    initToastContainer();
    
    const toast = document.createElement('div');
    toast.className = `toast ${type}`;
    
    const titles = {
        success: 'Успешно',
        error: 'Ошибка',
        warning: 'Внимание',
        info: 'Информация'
    };
    
    toast.innerHTML = `
        <div class="toast-header">
            <span class="toast-title">${titles[type] || 'Уведомление'}</span>
            <button class="toast-close">&times;</button>
        </div>
        <div class="toast-body">${escapeHtml(message)}</div>
    `;
    
    toastContainer.appendChild(toast);
    
    // Обработчик закрытия
    const closeBtn = toast.querySelector('.toast-close');
    closeBtn.addEventListener('click', () => {
        toast.remove();
    });
    
    // Автоматическое удаление
    if (duration > 0) {
        setTimeout(() => {
            toast.style.opacity = '0';
            setTimeout(() => toast.remove(), 300);
        }, duration);
    }
}

// Показать loader
function showLoader() {
    let loader = document.getElementById('globalLoader');
    if (!loader) {
        loader = document.createElement('div');
        loader.id = 'globalLoader';
        loader.className = 'loader-overlay';
        loader.innerHTML = '<div class="loader"></div>';
        document.body.appendChild(loader);
    }
    loader.style.display = 'flex';
}

// Скрыть loader
function hideLoader() {
    const loader = document.getElementById('globalLoader');
    if (loader) {
        loader.style.display = 'none';
    }
}

// Показать модальное окно
function showModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.add('active');
        document.body.style.overflow = 'hidden';
    }
}

// Скрыть модальное окно
function hideModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.remove('active');
        document.body.style.overflow = '';
    }
}

// Создать модальное окно программно
function createModal(title, content, actions = []) {
    const modalId = `modal-${generateId()}`;
    
    const modal = document.createElement('div');
    modal.id = modalId;
    modal.className = 'modal';
    
    let actionsHtml = '';
    if (actions.length > 0) {
        actionsHtml = '<div class="form-actions">';
        actions.forEach(action => {
            actionsHtml += `<button class="btn ${action.class || 'btn-primary'}" data-action="${action.action}">${action.label}</button>`;
        });
        actionsHtml += '</div>';
    }
    
    modal.innerHTML = `
        <div class="modal-content">
            <span class="modal-close">&times;</span>
            <h2>${title}</h2>
            <div class="modal-body">${content}</div>
            ${actionsHtml}
        </div>
    `;
    
    document.body.appendChild(modal);
    
    // Обработчик закрытия
    const closeBtn = modal.querySelector('.modal-close');
    closeBtn.addEventListener('click', () => {
        hideModal(modalId);
        setTimeout(() => modal.remove(), 300);
    });
    
    // Закрытие по клику вне модального окна
    modal.addEventListener('click', (e) => {
        if (e.target === modal) {
            hideModal(modalId);
            setTimeout(() => modal.remove(), 300);
        }
    });
    
    // Обработчики действий
    actions.forEach(action => {
        const btn = modal.querySelector(`[data-action="${action.action}"]`);
        if (btn && action.handler) {
            btn.addEventListener('click', () => {
                action.handler(modal, modalId);
            });
        }
    });
    
    showModal(modalId);
    return modalId;
}

// Диалог подтверждения
function confirmDialog(message, onConfirm, onCancel = null) {
    const modalId = createModal(
        'Подтверждение',
        `<p>${escapeHtml(message)}</p>`,
        [
            {
                label: 'Отмена',
                class: 'btn-secondary',
                action: 'cancel',
                handler: (modal, id) => {
                    hideModal(id);
                    setTimeout(() => modal.remove(), 300);
                    if (onCancel) onCancel();
                }
            },
            {
                label: 'Подтвердить',
                class: 'btn-primary',
                action: 'confirm',
                handler: (modal, id) => {
                    hideModal(id);
                    setTimeout(() => modal.remove(), 300);
                    if (onConfirm) onConfirm();
                }
            }
        ]
    );
}

// Создать таблицу
function createTable(columns, data, options = {}) {
    const {
        emptyMessage = 'Нет данных для отображения',
        rowActions = null,
        onRowClick = null
    } = options;
    
    if (!data || data.length === 0) {
        return `
            <div class="empty-state">
                <div class="empty-state-icon">📭</div>
                <h3>Нет данных</h3>
                <p>${emptyMessage}</p>
            </div>
        `;
    }
    
    let html = '<table><thead><tr>';
    
    // Заголовки
    columns.forEach(col => {
        html += `<th>${col.label}</th>`;
    });
    
    if (rowActions) {
        html += '<th>Действия</th>';
    }
    
    html += '</tr></thead><tbody>';
    
    // Строки данных
    data.forEach((row, index) => {
        const rowClass = onRowClick ? 'cursor-pointer' : '';
        const rowClickAttr = onRowClick ? `onclick="(${onRowClick})(${index})"` : '';
        
        html += `<tr class="${rowClass}" ${rowClickAttr}>`;
        
        columns.forEach(col => {
            let value = getNestedProperty(row, col.field, '-');
            
            // Применить форматирование если есть
            if (col.format) {
                value = col.format(value, row);
            }
            
            html += `<td>${value}</td>`;
        });
        
        // Действия
        if (rowActions) {
            html += '<td><div class="table-actions">';
            rowActions.forEach(action => {
                const disabled = action.disabled && action.disabled(row) ? 'disabled' : '';
                html += `<button class="btn ${action.class || 'btn-sm btn-primary'}" 
                                onclick="(${action.handler})(${index})" 
                                ${disabled}>
                            ${action.label}
                        </button>`;
            });
            html += '</div></td>';
        }
        
        html += '</tr>';
    });
    
    html += '</tbody></table>';
    return html;
}

// Создать пагинацию
function createPagination(currentPage, totalPages, onPageChange) {
    if (totalPages <= 1) return '';
    
    let html = '<div class="pagination">';
    
    // Кнопка "Назад"
    const prevDisabled = currentPage === 1 ? 'disabled' : '';
    html += `<button class="pagination-btn" onclick="(${onPageChange})(${currentPage - 1})" ${prevDisabled}>‹ Назад</button>`;
    
    // Номера страниц
    const maxButtons = 5;
    let startPage = Math.max(1, currentPage - Math.floor(maxButtons / 2));
    let endPage = Math.min(totalPages, startPage + maxButtons - 1);
    
    if (endPage - startPage < maxButtons - 1) {
        startPage = Math.max(1, endPage - maxButtons + 1);
    }
    
    if (startPage > 1) {
        html += `<button class="pagination-btn" onclick="(${onPageChange})(1)">1</button>`;
        if (startPage > 2) {
            html += '<span class="pagination-info">...</span>';
        }
    }
    
    for (let i = startPage; i <= endPage; i++) {
        const activeClass = i === currentPage ? 'active' : '';
        html += `<button class="pagination-btn ${activeClass}" onclick="(${onPageChange})(${i})">${i}</button>`;
    }
    
    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            html += '<span class="pagination-info">...</span>';
        }
        html += `<button class="pagination-btn" onclick="(${onPageChange})(${totalPages})">${totalPages}</button>`;
    }
    
    // Кнопка "Вперед"
    const nextDisabled = currentPage === totalPages ? 'disabled' : '';
    html += `<button class="pagination-btn" onclick="(${onPageChange})(${currentPage + 1})" ${nextDisabled}>Вперед ›</button>`;
    
    html += '</div>';
    return html;
}

// Проверка авторизации
function checkAuth() {
    if (!authApi.isAuthenticated()) {
        showModal('authModal');
        return false;
    }
    
    // Установить имя пользователя
    const userName = authApi.getUserName();
    const userNameElement = document.getElementById('userName');
    if (userNameElement) {
        userNameElement.textContent = userName;
    }
    
    return true;
}

// Выход из системы
function logout() {
    confirmDialog('Вы уверены, что хотите выйти?', () => {
        authApi.logout();
    });
}

// Обработчик формы авторизации
document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            
            const username = document.getElementById('username').value;
            const password = document.getElementById('password').value;
            
            try {
                const result = await authApi.login(username, password);
                
                if (result.success) {
                    hideModal('authModal');
                    showToast('Вход выполнен успешно', 'success');
                    
                    // Обновить имя пользователя
                    const userNameElement = document.getElementById('userName');
                    if (userNameElement) {
                        userNameElement.textContent = result.userName;
                    }
                    
                    // Перезагрузить страницу если нужно
                    if (window.location.pathname !== '/index.html' && window.location.pathname !== '/') {
                        location.reload();
                    }
                } else {
                    showToast(result.message || 'Ошибка входа', 'error');
                }
            } catch (error) {
                showToast('Ошибка при входе в систему', 'error');
            }
        });
    }
    
    // Обработчик ссылки на регистрацию
    const showRegisterLink = document.getElementById('showRegister');
    if (showRegisterLink) {
        showRegisterLink.addEventListener('click', (e) => {
            e.preventDefault();
            showToast('Регистрация временно недоступна. Обратитесь к администратору.', 'info');
        });
    }
});
