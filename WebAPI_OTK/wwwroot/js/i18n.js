// ========================================
// ЛОКАЛИЗАЦИЯ (i18n)
// ========================================

const translations = {
    ru: {
        // Навигация
        'nav.home': 'Главная',
        'nav.mlClosure': 'Закрытие МЛ',
        'nav.premKoef': 'Премиальные коэффициенты',
        'nav.dceCatalog': 'Каталог ДСЕ',
        'nav.logout': 'Выйти',
        
        // Главная страница
        'home.welcome': 'Добро пожаловать в систему учета ОТК',
        'home.mlClosureTitle': 'Закрытие МЛ',
        'home.mlClosureDesc': 'Поиск и закрытие маршрутных листов и операций',
        'home.premKoefTitle': 'Премиальные коэффициенты',
        'home.premKoefDesc': 'Управление коэффициентами премирования',
        'home.dceCatalogTitle': 'Каталог ДСЕ',
        'home.dceCatalogDesc': 'Просмотр деталей и сборочных единиц',
        
        // Страница закрытия МЛ
        'mlClosure.title': 'Закрытие маршрутных листов',
        'mlClosure.search': 'Поиск маршрутного листа',
        'mlClosure.searchType': 'Тип поиска',
        'mlClosure.searchByMl': 'По номеру МЛ',
        'mlClosure.searchByProduct': 'По изделию',
        'mlClosure.searchByDce': 'По ДСЕ',
        'mlClosure.mlNumber': 'Номер МЛ',
        'mlClosure.mlNumberPlaceholder': 'Введите номер МЛ',
        'mlClosure.product': 'Изделие',
        'mlClosure.selectProduct': 'Выберите изделие...',
        'mlClosure.dce': 'ДСЕ',
        'mlClosure.selectDce': 'Выберите ДСЕ...',
        'mlClosure.status': 'Статус',
        'mlClosure.statusAll': 'Все',
        'mlClosure.statusOpen': 'Открытые',
        'mlClosure.statusClosed': 'Закрытые',
        'mlClosure.searchBtn': 'Найти',
        'mlClosure.foundMl': 'Найденные маршрутные листы',
        'mlClosure.mlDetails': 'Маршрутный лист',
        'mlClosure.closeMlBtn': 'Закрыть МЛ',
        'mlClosure.backToList': 'Назад к списку',
        'mlClosure.dateCreated': 'Дата создания',
        'mlClosure.otkEmployee': 'Сотрудник ОТК',
        'mlClosure.operationFilters': 'Фильтры операций',
        'mlClosure.operationStatus': 'Статус операции',
        'mlClosure.employee': 'Сотрудник',
        'mlClosure.allEmployees': 'Все сотрудники',
        'mlClosure.dateFrom': 'Дата с',
        'mlClosure.dateTo': 'Дата по',
        
        // Таблица МЛ
        'table.mlNumber': 'Номер МЛ',
        'table.product': 'Изделие',
        'table.dce': 'ДСЕ',
        'table.dateCreated': 'Дата создания',
        'table.status': 'Статус',
        'table.operations': 'Операций',
        'table.statusOpen': 'Открыт',
        'table.statusClosed': 'Закрыт',
        
        // Таблица операций
        'table.operationType': 'Тип операции',
        'table.employee': 'Сотрудник',
        'table.quantity': 'Кол-во',
        'table.timeStandard': 'Норма (ч)',
        'table.tariff': 'Тариф',
        'table.pricePerHour': 'Цена (₽/ч)',
        'table.executionDate': 'Дата Исполнения',
        'table.closureDate': 'Дата Закрытия',
        'table.baseSum': 'Базовая (₽)',
        'table.dealCoef': 'Коэф. сделки',
        'table.surcharge': 'Надбавки (₽)',
        'table.premiumCoef': 'Коэф. премии',
        'table.premium': 'Премия (₽)',
        'table.total': 'Итого (₽)',
        'table.actions': 'Действия',
        
        // Действия
        'action.showCalculation': 'Показать расчет',
        'action.finish': 'Завершить',
        'action.cancel': 'Отменить',
        'action.delete': 'Удалить',
        
        // Модальное окно закрытия МЛ
        'modal.closeMl.title': 'Закрытие маршрутного листа',
        'modal.closeMl.otkEmployee': 'Сотрудник ОТК',
        'modal.closeMl.selectEmployee': 'Выберите сотрудника...',
        'modal.closeMl.otkQuantity': 'Количество ОТК',
        'modal.closeMl.defectQuantity': 'Количество брака',
        'modal.closeMl.cancelBtn': 'Отмена',
        'modal.closeMl.submitBtn': 'Закрыть МЛ',
        
        // Модальное окно расчета
        'modal.calculation.title': 'Детали расчета операции',
        'modal.calculation.inputData': 'Входные данные',
        'modal.calculation.quantity': 'Количество',
        'modal.calculation.timeStandard': 'Норма времени (ч)',
        'modal.calculation.pricePerHour': 'Цена за час (₽/ч)',
        'modal.calculation.dealCoef': 'Коэффициент сделки',
        'modal.calculation.executionDate': 'Дата исполнения',
        'modal.calculation.closureDate': 'Дата закрытия',
        'modal.calculation.onTime': 'Выполнено в срок',
        'modal.calculation.yes': 'Да',
        'modal.calculation.no': 'Нет',
        'modal.calculation.coefSource': 'Источник коэффициента сделки',
        'modal.calculation.product': 'Изделие',
        'modal.calculation.dce': 'ДСЕ',
        'modal.calculation.operationType': 'Тип операции',
        'modal.calculation.coefficient': 'Коэффициент',
        'modal.calculation.validPeriod': 'Период действия',
        'modal.calculation.indefinite': 'бессрочно',
        'modal.calculation.notFound': 'Премиальный коэффициент не найден. Используется значение по умолчанию: 1.0',
        'modal.calculation.formulas': 'Формулы расчета',
        'modal.calculation.baseSum': 'Базовая сумма',
        'modal.calculation.baseSumFormula': 'Базовая сумма = Количество × Норма времени (ч) × Цена за час (₽/ч)',
        'modal.calculation.surcharge': 'Сумма надбавки',
        'modal.calculation.surchargeFormula': 'Сумма надбавки = (Коэффициент сделки - 1) × Базовая сумма',
        'modal.calculation.premiumCoef': 'Коэффициент премии',
        'modal.calculation.premiumCoefFormula': 'Коэффициент премии = 0.8 (если в срок) или 0 (если просрочено)',
        'modal.calculation.premium': 'Сумма премии',
        'modal.calculation.premiumFormula': 'Сумма премии = Коэффициент премии × (Сумма надбавки + Базовая сумма)',
        'modal.calculation.totalSum': 'Итоговая сумма',
        'modal.calculation.totalFormula': 'Итого = Базовая сумма + Сумма надбавки + Сумма премии',
        'modal.calculation.onTimeText': 'в срок',
        'modal.calculation.lateText': 'с просрочкой',
        
        // Модальное окно авторизации
        'modal.auth.title': 'Вход в систему',
        'modal.auth.username': 'Логин',
        'modal.auth.password': 'Пароль',
        'modal.auth.loginBtn': 'Войти',
        
        // Статусы
        'status.inProgress': 'В работе',
        'status.completed': 'Завершена',
        'status.cancelled': 'Отменена',
        'status.waiting': 'Ожидание',
        
        // Сообщения
        'message.noData': 'Нет данных для отображения',
        'message.noMl': 'Маршрутные листы не найдены',
        'message.tryChangeFilters': 'Попробуйте изменить параметры поиска',
        'message.noOperations': 'Нет операций',
        'message.noOperationsForMl': 'Для этого МЛ еще не добавлены операции',
        'message.emptyDatabase': 'База данных пуста. Добавьте данные через API или SQL.',
        'message.serverError': 'Не удалось подключиться к серверу. Проверьте, что сервер запущен.',
        'message.operationFinished': 'Операция успешно завершена',
        'message.operationCancelled': 'Операция отменена',
        'message.mlClosed': 'Маршрутный лист успешно закрыт',
        'message.selectEmployee': 'Выберите сотрудника ОТК',
        'message.selectProduct': 'Выберите изделие',
        'message.selectDce': 'Выберите ДСЕ',
        'message.errorLoadingOperations': 'Ошибка загрузки операций МЛ',
        'message.errorLoadingDce': 'Ошибка загрузки ДСЕ',
        'message.errorSearchMl': 'Ошибка при поиске маршрутных листов',
        'message.errorFinishOperation': 'Ошибка при завершении операции',
        'message.errorCancelOperation': 'Ошибка при отмене операции',
        'message.errorCloseMl': 'Ошибка при закрытии МЛ',
        'message.errorLoadingCalculation': 'Ошибка при загрузке деталей расчета',
        'message.loginSuccess': 'Вход выполнен успешно',
        'message.loginError': 'Ошибка входа',
        'message.logoutConfirm': 'Вы уверены, что хотите выйти?',
        
        // Подтверждения
        'confirm.finishOperation': 'Завершить операцию',
        'confirm.cancelOperation': 'Отменить операцию',
        'confirm.cancelOperationText': 'Операция будет помечена как отмененная. Это действие нельзя отменить.',
        'confirm.yes': 'Подтвердить',
        'confirm.no': 'Отмена',
        'confirm.title': 'Подтверждение',
        
        // Toast заголовки
        'toast.success': 'Успешно',
        'toast.error': 'Ошибка',
        'toast.warning': 'Внимание',
        'toast.info': 'Информация',
        'toast.notification': 'Уведомление',
        
        // Пагинация
        'pagination.prev': '‹ Назад',
        'pagination.next': 'Вперед ›',
        'pagination.recordsPerPage': 'Записей на странице',
        'pagination.of': 'из',
        'pagination.showing': 'Показано',
        'pagination.previous': '← Предыдущая',
        'pagination.nextFull': 'Следующая →',
        
        // Премиальные коэффициенты
        'premKoef.title': 'Премиальные коэффициенты',
        'premKoef.totalCoefficients': 'Всего коэффициентов',
        'premKoef.active': 'Активных',
        'premKoef.inactive': 'Неактивных',
        'premKoef.filtersAndSearch': 'Фильтры и поиск',
        'premKoef.addCoefficient': '+ Добавить коэффициент',
        'premKoef.searchByName': 'Поиск по наименованию',
        'premKoef.searchPlaceholder': 'Введите название...',
        'premKoef.statusFilter': 'Статус',
        'premKoef.statusAll': 'Все',
        'premKoef.statusActive': 'Только активные',
        'premKoef.statusInactive': 'Только неактивные',
        'premKoef.productFilter': 'Изделие',
        'premKoef.allProducts': 'Все изделия',
        'premKoef.operationTypeFilter': 'Тип операции',
        'premKoef.allTypes': 'Все типы',
        'premKoef.dateCheck': 'Дата проверки',
        'premKoef.resetFilters': 'Сбросить фильтры',
        'premKoef.notFound': 'Коэффициенты не найдены',
        'premKoef.tryChangeFilters': 'Попробуйте изменить параметры фильтрации или добавьте новый коэффициент',
        'premKoef.validPeriod': 'Период действия',
        'premKoef.indefinite': 'Бессрочно',
        'premKoef.product': 'Изделие',
        'premKoef.dce': 'ДСЕ',
        'premKoef.operationType': 'Тип операции',
        'premKoef.all': 'Все',
        'premKoef.statusActive': 'Активный',
        'premKoef.statusInactive': 'Неактивный',
        'premKoef.edit': '✏️ Редактировать',
        'premKoef.deactivate': '⏸ Деактивировать',
        'premKoef.delete': '🗑️ Удалить',
        
        // Модальное окно коэффициента
        'modal.koef.titleAdd': 'Добавить коэффициент',
        'modal.koef.titleEdit': 'Редактировать коэффициент',
        'modal.koef.name': 'Наименование',
        'modal.koef.coefficient': 'Коэффициент',
        'modal.koef.coefficientHint': 'Например: 1.5 означает +50% к базовой ставке',
        'modal.koef.startDate': 'Дата начала',
        'modal.koef.endDate': 'Дата окончания',
        'modal.koef.endDateHint': 'Оставьте пустым для бессрочного',
        'modal.koef.product': 'Изделие',
        'modal.koef.productNotSpecified': 'Не указано (для всех)',
        'modal.koef.dce': 'ДСЕ',
        'modal.koef.dceNotSpecified': 'Не указано (для всех)',
        'modal.koef.operationType': 'Тип операции',
        'modal.koef.operationTypeNotSpecified': 'Не указано (для всех)',
        'modal.koef.cancel': 'Отмена',
        'modal.koef.save': 'Сохранить',
        
        // Сообщения коэффициентов
        'message.koefCreated': 'Коэффициент успешно создан',
        'message.koefUpdated': 'Коэффициент успешно обновлен',
        'message.koefDeactivated': 'Коэффициент успешно деактивирован',
        'message.koefDeleted': 'Коэффициент успешно удален',
        'message.errorLoadingKoef': 'Ошибка загрузки данных коэффициента',
        'message.errorSavingKoef': 'Ошибка при сохранении коэффициента',
        'message.errorDeactivatingKoef': 'Ошибка при деактивации коэффициента',
        'message.errorDeletingKoef': 'Ошибка при удалении коэффициента',
        'message.errorLoadingKoefs': 'Ошибка загрузки коэффициентов',
        
        // Подтверждения коэффициентов
        'confirm.deactivateKoef': 'Деактивировать коэффициент? Он перестанет действовать с текущей даты.',
        'confirm.deleteKoef': 'Удалить коэффициент? Это действие нельзя отменить.',
        
        // Каталог ДСЕ
        'dceCatalog.title': 'Каталог ДСЕ',
        'dceCatalog.searchByCodeOrName': 'Поиск по коду или наименованию',
        'dceCatalog.searchPlaceholder': 'Введите код или название...',
        'dceCatalog.product': 'Изделие',
        'dceCatalog.allProducts': 'Все изделия',
        'dceCatalog.recordsPerPage': 'Записей на странице',
        'dceCatalog.reset': 'Сбросить',
        'dceCatalog.loading': 'Загрузка...',
        'dceCatalog.code': 'Код',
        'dceCatalog.name': 'Наименование',
        'dceCatalog.actions': 'Действия',
        'dceCatalog.details': '👁️ Подробнее',
        'dceCatalog.notFound': 'ДСЕ не найдены',
        'dceCatalog.tryChangeSearch': 'Попробуйте изменить параметры поиска',
        'dceCatalog.modalTitle': 'Детали ДСЕ',
        'dceCatalog.id': 'ID',
        
        // Сообщения ДСЕ
        'message.errorLoadingDceList': 'Ошибка загрузки каталога ДСЕ',
        'message.errorLoadingDceDetails': 'Ошибка загрузки деталей',
        'message.errorLoadingReferenceData': 'Ошибка загрузки справочных данных'
    },
    
    en: {
        // Navigation
        'nav.home': 'Home',
        'nav.mlClosure': 'Route List Closure',
        'nav.premKoef': 'Premium Coefficients',
        'nav.dceCatalog': 'DCE Catalog',
        'nav.logout': 'Logout',
        
        // Home Page
        'home.welcome': 'Welcome to the QC Accounting System',
        'home.mlClosureTitle': 'Route List Closure',
        'home.mlClosureDesc': 'Search and close route lists and operations',
        'home.premKoefTitle': 'Premium Coefficients',
        'home.premKoefDesc': 'Manage premium coefficients',
        'home.dceCatalogTitle': 'DCE Catalog',
        'home.dceCatalogDesc': 'View parts and assemblies',
        
        // ML Closure Page
        'mlClosure.title': 'Route List Closure',
        'mlClosure.search': 'Search Route List',
        'mlClosure.searchType': 'Search Type',
        'mlClosure.searchByMl': 'By ML Number',
        'mlClosure.searchByProduct': 'By Product',
        'mlClosure.searchByDce': 'By DCE',
        'mlClosure.mlNumber': 'ML Number',
        'mlClosure.mlNumberPlaceholder': 'Enter ML number',
        'mlClosure.product': 'Product',
        'mlClosure.selectProduct': 'Select product...',
        'mlClosure.dce': 'DCE',
        'mlClosure.selectDce': 'Select DCE...',
        'mlClosure.status': 'Status',
        'mlClosure.statusAll': 'All',
        'mlClosure.statusOpen': 'Open',
        'mlClosure.statusClosed': 'Closed',
        'mlClosure.searchBtn': 'Search',
        'mlClosure.foundMl': 'Found Route Lists',
        'mlClosure.mlDetails': 'Route List',
        'mlClosure.closeMlBtn': 'Close ML',
        'mlClosure.backToList': 'Back to List',
        'mlClosure.dateCreated': 'Date Created',
        'mlClosure.otkEmployee': 'QC Employee',
        'mlClosure.operationFilters': 'Operation Filters',
        'mlClosure.operationStatus': 'Operation Status',
        'mlClosure.employee': 'Employee',
        'mlClosure.allEmployees': 'All Employees',
        'mlClosure.dateFrom': 'Date From',
        'mlClosure.dateTo': 'Date To',
        
        // Table ML
        'table.mlNumber': 'ML Number',
        'table.product': 'Product',
        'table.dce': 'DCE',
        'table.dateCreated': 'Date Created',
        'table.status': 'Status',
        'table.operations': 'Operations',
        'table.statusOpen': 'Open',
        'table.statusClosed': 'Closed',
        
        // Operations Table
        'table.operationType': 'Operation Type',
        'table.employee': 'Employee',
        'table.quantity': 'Qty',
        'table.timeStandard': 'Time Std (h)',
        'table.tariff': 'Tariff',
        'table.pricePerHour': 'Price (₽/h)',
        'table.executionDate': 'Execution Date',
        'table.closureDate': 'Closure Date',
        'table.baseSum': 'Base (₽)',
        'table.dealCoef': 'Deal Coef',
        'table.surcharge': 'Surcharge (₽)',
        'table.premiumCoef': 'Premium Coef',
        'table.premium': 'Premium (₽)',
        'table.total': 'Total (₽)',
        'table.actions': 'Actions',
        
        // Actions
        'action.showCalculation': 'Show Calculation',
        'action.finish': 'Finish',
        'action.cancel': 'Cancel',
        'action.delete': 'Delete',
        
        // Close ML Modal
        'modal.closeMl.title': 'Close Route List',
        'modal.closeMl.otkEmployee': 'QC Employee',
        'modal.closeMl.selectEmployee': 'Select employee...',
        'modal.closeMl.otkQuantity': 'QC Quantity',
        'modal.closeMl.defectQuantity': 'Defect Quantity',
        'modal.closeMl.cancelBtn': 'Cancel',
        'modal.closeMl.submitBtn': 'Close ML',
        
        // Calculation Modal
        'modal.calculation.title': 'Operation Calculation Details',
        'modal.calculation.inputData': 'Input Data',
        'modal.calculation.quantity': 'Quantity',
        'modal.calculation.timeStandard': 'Time Standard (h)',
        'modal.calculation.pricePerHour': 'Price per Hour (₽/h)',
        'modal.calculation.dealCoef': 'Deal Coefficient',
        'modal.calculation.executionDate': 'Execution Date',
        'modal.calculation.closureDate': 'Closure Date',
        'modal.calculation.onTime': 'On Time',
        'modal.calculation.yes': 'Yes',
        'modal.calculation.no': 'No',
        'modal.calculation.coefSource': 'Deal Coefficient Source',
        'modal.calculation.product': 'Product',
        'modal.calculation.dce': 'DCE',
        'modal.calculation.operationType': 'Operation Type',
        'modal.calculation.coefficient': 'Coefficient',
        'modal.calculation.validPeriod': 'Valid Period',
        'modal.calculation.indefinite': 'indefinite',
        'modal.calculation.notFound': 'Premium coefficient not found. Using default value: 1.0',
        'modal.calculation.formulas': 'Calculation Formulas',
        'modal.calculation.baseSum': 'Base Sum',
        'modal.calculation.baseSumFormula': 'Base Sum = Quantity × Time Standard (h) × Price per Hour (₽/h)',
        'modal.calculation.surcharge': 'Surcharge',
        'modal.calculation.surchargeFormula': 'Surcharge = (Deal Coefficient - 1) × Base Sum',
        'modal.calculation.premiumCoef': 'Premium Coefficient',
        'modal.calculation.premiumCoefFormula': 'Premium Coefficient = 0.8 (if on time) or 0 (if late)',
        'modal.calculation.premium': 'Premium Sum',
        'modal.calculation.premiumFormula': 'Premium Sum = Premium Coefficient × (Surcharge + Base Sum)',
        'modal.calculation.totalSum': 'Total Sum',
        'modal.calculation.totalFormula': 'Total = Base Sum + Surcharge + Premium Sum',
        'modal.calculation.onTimeText': 'on time',
        'modal.calculation.lateText': 'late',
        
        // Auth Modal
        'modal.auth.title': 'Login',
        'modal.auth.username': 'Username',
        'modal.auth.password': 'Password',
        'modal.auth.loginBtn': 'Login',
        
        // Statuses
        'status.inProgress': 'In Progress',
        'status.completed': 'Completed',
        'status.cancelled': 'Cancelled',
        'status.waiting': 'Waiting',
        
        // Messages
        'message.noData': 'No data to display',
        'message.noMl': 'Route lists not found',
        'message.tryChangeFilters': 'Try changing search parameters',
        'message.noOperations': 'No Operations',
        'message.noOperationsForMl': 'No operations added for this ML yet',
        'message.emptyDatabase': 'Database is empty. Add data via API or SQL.',
        'message.serverError': 'Could not connect to server. Check that server is running.',
        'message.operationFinished': 'Operation finished successfully',
        'message.operationCancelled': 'Operation cancelled',
        'message.mlClosed': 'Route list closed successfully',
        'message.selectEmployee': 'Select QC employee',
        'message.selectProduct': 'Select product',
        'message.selectDce': 'Select DCE',
        'message.errorLoadingOperations': 'Error loading ML operations',
        'message.errorLoadingDce': 'Error loading DCE',
        'message.errorSearchMl': 'Error searching route lists',
        'message.errorFinishOperation': 'Error finishing operation',
        'message.errorCancelOperation': 'Error cancelling operation',
        'message.errorCloseMl': 'Error closing ML',
        'message.errorLoadingCalculation': 'Error loading calculation details',
        'message.loginSuccess': 'Login successful',
        'message.loginError': 'Login error',
        'message.logoutConfirm': 'Are you sure you want to logout?',
        
        // Confirmations
        'confirm.finishOperation': 'Finish operation',
        'confirm.cancelOperation': 'Cancel operation',
        'confirm.cancelOperationText': 'Operation will be marked as cancelled. This action cannot be undone.',
        'confirm.yes': 'Confirm',
        'confirm.no': 'Cancel',
        'confirm.title': 'Confirmation',
        
        // Toast titles
        'toast.success': 'Success',
        'toast.error': 'Error',
        'toast.warning': 'Warning',
        'toast.info': 'Information',
        'toast.notification': 'Notification',
        
        // Pagination
        'pagination.prev': '‹ Previous',
        'pagination.next': 'Next ›',
        'pagination.recordsPerPage': 'Records per page',
        'pagination.of': 'of',
        'pagination.showing': 'Showing',
        'pagination.previous': '← Previous',
        'pagination.nextFull': 'Next →',
        
        // Premium Coefficients
        'premKoef.title': 'Premium Coefficients',
        'premKoef.totalCoefficients': 'Total Coefficients',
        'premKoef.active': 'Active',
        'premKoef.inactive': 'Inactive',
        'premKoef.filtersAndSearch': 'Filters and Search',
        'premKoef.addCoefficient': '+ Add Coefficient',
        'premKoef.searchByName': 'Search by name',
        'premKoef.searchPlaceholder': 'Enter name...',
        'premKoef.statusFilter': 'Status',
        'premKoef.statusAll': 'All',
        'premKoef.statusActive': 'Active only',
        'premKoef.statusInactive': 'Inactive only',
        'premKoef.productFilter': 'Product',
        'premKoef.allProducts': 'All products',
        'premKoef.operationTypeFilter': 'Operation Type',
        'premKoef.allTypes': 'All types',
        'premKoef.dateCheck': 'Check Date',
        'premKoef.resetFilters': 'Reset Filters',
        'premKoef.notFound': 'Coefficients not found',
        'premKoef.tryChangeFilters': 'Try changing filter parameters or add a new coefficient',
        'premKoef.validPeriod': 'Valid Period',
        'premKoef.indefinite': 'Indefinite',
        'premKoef.product': 'Product',
        'premKoef.dce': 'DCE',
        'premKoef.operationType': 'Operation Type',
        'premKoef.all': 'All',
        'premKoef.statusActive': 'Active',
        'premKoef.statusInactive': 'Inactive',
        'premKoef.edit': '✏️ Edit',
        'premKoef.deactivate': '⏸ Deactivate',
        'premKoef.delete': '🗑️ Delete',
        
        // Coefficient Modal
        'modal.koef.titleAdd': 'Add Coefficient',
        'modal.koef.titleEdit': 'Edit Coefficient',
        'modal.koef.name': 'Name',
        'modal.koef.coefficient': 'Coefficient',
        'modal.koef.coefficientHint': 'Example: 1.5 means +50% to base rate',
        'modal.koef.startDate': 'Start Date',
        'modal.koef.endDate': 'End Date',
        'modal.koef.endDateHint': 'Leave empty for indefinite',
        'modal.koef.product': 'Product',
        'modal.koef.productNotSpecified': 'Not specified (for all)',
        'modal.koef.dce': 'DCE',
        'modal.koef.dceNotSpecified': 'Not specified (for all)',
        'modal.koef.operationType': 'Operation Type',
        'modal.koef.operationTypeNotSpecified': 'Not specified (for all)',
        'modal.koef.cancel': 'Cancel',
        'modal.koef.save': 'Save',
        
        // Coefficient Messages
        'message.koefCreated': 'Coefficient created successfully',
        'message.koefUpdated': 'Coefficient updated successfully',
        'message.koefDeactivated': 'Coefficient deactivated successfully',
        'message.koefDeleted': 'Coefficient deleted successfully',
        'message.errorLoadingKoef': 'Error loading coefficient data',
        'message.errorSavingKoef': 'Error saving coefficient',
        'message.errorDeactivatingKoef': 'Error deactivating coefficient',
        'message.errorDeletingKoef': 'Error deleting coefficient',
        'message.errorLoadingKoefs': 'Error loading coefficients',
        
        // Coefficient Confirmations
        'confirm.deactivateKoef': 'Deactivate coefficient? It will stop being effective from current date.',
        'confirm.deleteKoef': 'Delete coefficient? This action cannot be undone.',
        
        // DCE Catalog
        'dceCatalog.title': 'DCE Catalog',
        'dceCatalog.searchByCodeOrName': 'Search by code or name',
        'dceCatalog.searchPlaceholder': 'Enter code or name...',
        'dceCatalog.product': 'Product',
        'dceCatalog.allProducts': 'All products',
        'dceCatalog.recordsPerPage': 'Records per page',
        'dceCatalog.reset': 'Reset',
        'dceCatalog.loading': 'Loading...',
        'dceCatalog.code': 'Code',
        'dceCatalog.name': 'Name',
        'dceCatalog.actions': 'Actions',
        'dceCatalog.details': '👁️ Details',
        'dceCatalog.notFound': 'DCE not found',
        'dceCatalog.tryChangeSearch': 'Try changing search parameters',
        'dceCatalog.modalTitle': 'DCE Details',
        'dceCatalog.id': 'ID',
        
        // DCE Messages
        'message.errorLoadingDceList': 'Error loading DCE catalog',
        'message.errorLoadingDceDetails': 'Error loading details',
        'message.errorLoadingReferenceData': 'Error loading reference data'
    }
};

// Текущий язык
let currentLanguage = localStorage.getItem('language') || 'ru';

// Получить перевод
function t(key) {
    return translations[currentLanguage]?.[key] || key;
}

// Установить язык
function setLanguage(lang) {
    if (!translations[lang]) {
        console.warn(`Language ${lang} not supported`);
        return;
    }
    
    currentLanguage = lang;
    localStorage.setItem('language', lang);
    
    // Обновить все элементы с data-i18n
    updatePageTranslations();
    
    // Обновить кнопки языка
    updateLanguageButtons();
    
    // Триггер события для обновления динамического контента
    document.dispatchEvent(new CustomEvent('languageChanged', { detail: { language: lang } }));
}

// Получить текущий язык
function getLanguage() {
    return currentLanguage;
}

// Обновить переводы на странице
function updatePageTranslations() {
    document.querySelectorAll('[data-i18n]').forEach(element => {
        const key = element.getAttribute('data-i18n');
        const translation = t(key);
        
        if (element.tagName === 'INPUT' || element.tagName === 'TEXTAREA') {
            if (element.placeholder !== undefined) {
                element.placeholder = translation;
            }
        } else {
            element.textContent = translation;
        }
    });
    
    // Обновить placeholder отдельно
    document.querySelectorAll('[data-i18n-placeholder]').forEach(element => {
        const key = element.getAttribute('data-i18n-placeholder');
        element.placeholder = t(key);
    });
    
    // Обновить title атрибуты
    document.querySelectorAll('[data-i18n-title]').forEach(element => {
        const key = element.getAttribute('data-i18n-title');
        element.title = t(key);
    });
}

// Обновить активную кнопку языка
function updateLanguageButtons() {
    document.querySelectorAll('.lang-btn').forEach(btn => {
        btn.classList.remove('active');
    });
    
    const activeLangBtn = document.getElementById(`lang${currentLanguage.charAt(0).toUpperCase() + currentLanguage.slice(1)}`);
    if (activeLangBtn) {
        activeLangBtn.classList.add('active');
    }
}

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', () => {
    updatePageTranslations();
    updateLanguageButtons();
});
