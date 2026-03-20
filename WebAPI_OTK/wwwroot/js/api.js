// ========================================
// API CLIENT
// ========================================

const API_BASE_URL = window.location.origin + '/api';

// Класс для работы с API
class ApiClient {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
    }

    // Получение токена из localStorage
    getToken() {
        return localStorage.getItem('authToken');
    }

    // Установка токена
    setToken(token) {
        localStorage.setItem('authToken', token);
    }

    // Удаление токена
    removeToken() {
        localStorage.removeItem('authToken');
    }

    // Базовый метод для выполнения запросов
    async request(endpoint, options = {}) {
        const url = `${this.baseUrl}${endpoint}`;
        const token = this.getToken();

        const headers = {
            'Content-Type': 'application/json',
            ...options.headers
        };

        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        const config = {
            ...options,
            headers
        };

        try {
            showLoader();
            console.log('API Request:', config.method || 'GET', url);
            const response = await fetch(url, config);
            
            // Если 401 - перенаправляем на авторизацию
            if (response.status === 401) {
                this.removeToken();
                window.location.href = '/index.html';
                return null;
            }

            if (!response.ok) {
                let errorMessage = `HTTP Error: ${response.status}`;
                try {
                    const error = await response.json();
                    errorMessage = error.message || error.title || JSON.stringify(error);
                } catch (e) {
                    // Если не удалось распарсить JSON, используем текст ответа
                    try {
                        errorMessage = await response.text() || errorMessage;
                    } catch (e2) {
                        // Оставляем дефолтное сообщение
                    }
                }
                
                // Логируем ошибку, но не показываем toast автоматически
                console.error('API Error:', errorMessage);
                throw new Error(errorMessage);
            }

            // Если ответ пустой (204 No Content)
            if (response.status === 204) {
                return null;
            }

            return await response.json();
        } catch (error) {
            console.error('API Error:', error);
            
            // Проверка на ошибки сети - показываем toast только для критичных ошибок
            if (error.name === 'TypeError' && error.message.includes('fetch')) {
                showToast('Ошибка подключения к серверу. Проверьте соединение.', 'error');
            }
            // Для остальных ошибок не показываем toast автоматически
            // Вызывающий код сам решит, показывать ли сообщение
            
            throw error;
        } finally {
            hideLoader();
        }
    }

    // GET запрос
    async get(endpoint, params = {}) {
        const queryString = new URLSearchParams(params).toString();
        const url = queryString ? `${endpoint}?${queryString}` : endpoint;
        return this.request(url, { method: 'GET' });
    }

    // POST запрос
    async post(endpoint, data) {
        return this.request(endpoint, {
            method: 'POST',
            body: JSON.stringify(data)
        });
    }

    // PUT запрос
    async put(endpoint, data) {
        return this.request(endpoint, {
            method: 'PUT',
            body: JSON.stringify(data)
        });
    }

    // DELETE запрос
    async delete(endpoint) {
        return this.request(endpoint, { method: 'DELETE' });
    }
}

// Создаем экземпляр API клиента
const api = new ApiClient(API_BASE_URL);

// ========================================
// API МЕТОДЫ ДЛЯ МАРШРУТНЫХ ЛИСТОВ
// ========================================

const mlApi = {
    // Получить список МЛ с фильтрами
    getAll: (filters = {}) => api.get('/RouteList', filters),
    
    // Получить МЛ по ID
    getById: (id) => api.get(`/RouteList/${id}`),
    
    // Получить открытые МЛ
    getOpened: () => api.get('/RouteList/opened'),
    
    // Закрыть МЛ
    close: (id, data) => api.put(`/RouteList/close/${id}`, data)
};

// ========================================
// API МЕТОДЫ ДЛЯ ОПЕРАЦИЙ МЛ
// ========================================

const operationApi = {
    // Получить список операций с фильтрами
    getAll: (filters = {}) => api.get('/OperationRouteList', filters),
    
    // Получить операцию по ID
    getById: (id) => api.get(`/OperationRouteList/${id}`),
    
    // Получить операции сотрудника
    getByEmployee: (employeeId) => api.get(`/OperationRouteList/employee/${employeeId}`),
    
    // Получить детали расчета операции
    getCalculation: (id) => api.get(`/OperationRouteList/calculation/${id}`),
    
    // Создать операцию
    create: (data) => api.post('/OperationRouteList', data),
    
    // Обновить операцию
    update: (id, data) => api.put(`/OperationRouteList/${id}`, data),
    
    // Завершить операцию
    finish: (id) => api.put(`/OperationRouteList/finish/${id}`, {}),
    
    // Отменить операцию
    cancel: (id) => api.put(`/OperationRouteList/cancel/${id}`, {}),
    
    // Удалить операцию
    delete: (id) => api.delete(`/OperationRouteList/${id}`)
};

// ========================================
// API МЕТОДЫ ДЛЯ ПРЕМИАЛЬНЫХ КОЭФФИЦИЕНТОВ
// ========================================

const premKoefApi = {
    // Получить список коэффициентов
    getAll: (filters = {}) => api.get('/PremKoef', filters),
    
    // Получить коэффициент по ID
    getById: (id) => api.get(`/PremKoef/${id}`),
    
    // Создать коэффициент
    create: (data) => api.post('/PremKoef', data),
    
    // Обновить коэффициент
    update: (id, data) => api.put(`/PremKoef/${id}`, data),
    
    // Деактивировать коэффициент (установить дату окончания)
    deactivate: (id) => api.put(`/PremKoef/deactivate/${id}`, {}),
    
    // Удалить коэффициент (только неактивные)
    delete: (id) => api.delete(`/PremKoef/${id}`)
};

// ========================================
// API МЕТОДЫ ДЛЯ ДСЕ
// ========================================

const dceApi = {
    // Получить список ДСЕ с пагинацией
    getAll: (params = {}) => api.get('/DCE', params),
    
    // Получить ДСЕ по ID
    getById: (id) => api.get(`/DCE/${id}`)
};

// ========================================
// API МЕТОДЫ ДЛЯ СОТРУДНИКОВ
// ========================================

const employeeApi = {
    // Получить список сотрудников
    getAll: (filters = {}) => api.get('/Employee', filters),
    
    // Получить сотрудника по ID
    getById: (id) => api.get(`/Employee/${id}`)
};

// ========================================
// API МЕТОДЫ ДЛЯ ИЗДЕЛИЙ
// ========================================

const productApi = {
    // Получить список изделий
    getAll: () => api.get('/Product'),
    
    // Получить изделие по ID
    getById: (id) => api.get(`/Product/${id}`),
    
    // Получить ДСЕ изделия
    getDCE: (id) => api.get(`/Product/${id}/DCE`),
    
    // Получить МЛ изделия
    getML: (id) => api.get(`/Product/${id}/ML`)
};

// ========================================
// API МЕТОДЫ ДЛЯ ТИПОВ ОПЕРАЦИЙ
// ========================================

const operationTypeApi = {
    // Получить список типов операций
    getAll: () => api.get('/OperationType'),
    
    // Получить тип операции по ID
    getById: (id) => api.get(`/OperationType/${id}`)
};

// ========================================
// API МЕТОДЫ ДЛЯ АВТОРИЗАЦИИ (ЗАГЛУШКА)
// ========================================

const authApi = {
    // Вход
    login: async (username, password) => {
        // TODO: Заменить на реальный API когда будет готов
        // Временная заглушка
        return new Promise((resolve) => {
            setTimeout(() => {
                if (username && password) {
                    const fakeToken = btoa(`${username}:${Date.now()}`);
                    api.setToken(fakeToken);
                    localStorage.setItem('userName', username);
                    resolve({ success: true, token: fakeToken, userName: username });
                } else {
                    resolve({ success: false, message: 'Неверные учетные данные' });
                }
            }, 500);
        });
    },
    
    // Регистрация
    register: async (username, password, email) => {
        // TODO: Заменить на реальный API когда будет готов
        return new Promise((resolve) => {
            setTimeout(() => {
                if (username && password) {
                    resolve({ success: true, message: 'Регистрация успешна' });
                } else {
                    resolve({ success: false, message: 'Ошибка регистрации' });
                }
            }, 500);
        });
    },
    
    // Выход
    logout: () => {
        api.removeToken();
        localStorage.removeItem('userName');
        window.location.href = '/index.html';
    },
    
    // Проверка авторизации
    isAuthenticated: () => {
        return !!api.getToken();
    },
    
    // Получить имя пользователя
    getUserName: () => {
        return localStorage.getItem('userName') || 'Пользователь';
    }
};
