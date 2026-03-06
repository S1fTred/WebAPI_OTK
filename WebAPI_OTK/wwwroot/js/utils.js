// ========================================
// УТИЛИТЫ
// ========================================

// Форматирование даты
function formatDate(dateString) {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString('ru-RU', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    });
}

// Форматирование даты и времени
function formatDateTime(dateString) {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleString('ru-RU', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    });
}

// Форматирование числа с разделителями
function formatNumber(number, decimals = 2) {
    if (number === null || number === undefined) return '-';
    return Number(number).toLocaleString('ru-RU', {
        minimumFractionDigits: decimals,
        maximumFractionDigits: decimals
    });
}

// Форматирование валюты
function formatCurrency(amount) {
    if (amount === null || amount === undefined) return '-';
    return Number(amount).toLocaleString('ru-RU', {
        style: 'currency',
        currency: 'RUB'
    });
}

// Получение текущей даты в формате YYYY-MM-DD
function getCurrentDate() {
    const date = new Date();
    return date.toISOString().split('T')[0];
}

// Получение даты N дней назад
function getDateDaysAgo(days) {
    const date = new Date();
    date.setDate(date.getDate() - days);
    return date.toISOString().split('T')[0];
}

// Проверка, является ли дата действующей
function isDateActive(startDate, endDate) {
    const now = new Date();
    const start = startDate ? new Date(startDate) : null;
    const end = endDate ? new Date(endDate) : null;
    
    if (start && now < start) return false;
    if (end && now > end) return false;
    return true;
}

// Валидация email
function isValidEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

// Экранирование HTML
function escapeHtml(text) {
    const map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };
    return text.replace(/[&<>"']/g, m => map[m]);
}

// Дебаунс функции
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Получение параметров из URL
function getUrlParams() {
    const params = new URLSearchParams(window.location.search);
    const result = {};
    for (const [key, value] of params) {
        result[key] = value;
    }
    return result;
}

// Установка параметров в URL
function setUrlParams(params) {
    const url = new URL(window.location);
    Object.keys(params).forEach(key => {
        if (params[key]) {
            url.searchParams.set(key, params[key]);
        } else {
            url.searchParams.delete(key);
        }
    });
    window.history.pushState({}, '', url);
}

// Копирование текста в буфер обмена
async function copyToClipboard(text) {
    try {
        await navigator.clipboard.writeText(text);
        showToast('Скопировано в буфер обмена', 'success');
    } catch (err) {
        console.error('Ошибка копирования:', err);
        showToast('Ошибка копирования', 'error');
    }
}

// Скачивание файла
function downloadFile(data, filename, type = 'text/plain') {
    const blob = new Blob([data], { type });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
}

// Генерация уникального ID
function generateId() {
    return Date.now().toString(36) + Math.random().toString(36).substr(2);
}

// Задержка (для async/await)
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

// Группировка массива по ключу
function groupBy(array, key) {
    return array.reduce((result, item) => {
        const group = item[key];
        if (!result[group]) {
            result[group] = [];
        }
        result[group].push(item);
        return result;
    }, {});
}

// Сортировка массива объектов
function sortBy(array, key, order = 'asc') {
    return [...array].sort((a, b) => {
        const aVal = a[key];
        const bVal = b[key];
        
        if (aVal < bVal) return order === 'asc' ? -1 : 1;
        if (aVal > bVal) return order === 'asc' ? 1 : -1;
        return 0;
    });
}

// Фильтрация массива по поисковому запросу
function filterBySearch(array, searchTerm, fields) {
    if (!searchTerm) return array;
    
    const term = searchTerm.toLowerCase();
    return array.filter(item => {
        return fields.some(field => {
            const value = item[field];
            return value && value.toString().toLowerCase().includes(term);
        });
    });
}

// Пагинация массива
function paginate(array, page, pageSize) {
    const start = (page - 1) * pageSize;
    const end = start + pageSize;
    return {
        data: array.slice(start, end),
        total: array.length,
        page,
        pageSize,
        totalPages: Math.ceil(array.length / pageSize)
    };
}

// Проверка пустого значения
function isEmpty(value) {
    if (value === null || value === undefined) return true;
    if (typeof value === 'string') return value.trim() === '';
    if (Array.isArray(value)) return value.length === 0;
    if (typeof value === 'object') return Object.keys(value).length === 0;
    return false;
}

// Безопасное получение вложенного свойства
function getNestedProperty(obj, path, defaultValue = null) {
    const keys = path.split('.');
    let result = obj;
    
    for (const key of keys) {
        if (result === null || result === undefined) {
            return defaultValue;
        }
        result = result[key];
    }
    
    return result !== undefined ? result : defaultValue;
}

// Сравнение двух объектов
function isEqual(obj1, obj2) {
    return JSON.stringify(obj1) === JSON.stringify(obj2);
}

// Глубокое клонирование объекта
function deepClone(obj) {
    return JSON.parse(JSON.stringify(obj));
}

// Удаление пустых свойств из объекта
function removeEmptyProperties(obj) {
    const result = {};
    Object.keys(obj).forEach(key => {
        if (!isEmpty(obj[key])) {
            result[key] = obj[key];
        }
    });
    return result;
}

// Получение статуса операции
function getOperationStatus(operation) {
    if (!operation.статус) return 'unknown';
    return operation.статус.toLowerCase();
}

// Получение цвета статуса
function getStatusColor(status) {
    const colors = {
        'завершена': 'success',
        'в работе': 'warning',
        'отменена': 'error',
        'ожидание': 'info'
    };
    return colors[status.toLowerCase()] || 'neutral';
}

// Получение иконки статуса
function getStatusIcon(status) {
    const icons = {
        'завершена': '✓',
        'в работе': '⏳',
        'отменена': '✗',
        'ожидание': '⏸'
    };
    return icons[status.toLowerCase()] || '•';
}
