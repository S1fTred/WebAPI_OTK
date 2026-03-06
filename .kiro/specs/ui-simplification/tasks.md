# Implementation Plan: Упрощение интерфейса системы ОТК

## Overview

Реализация упрощения пользовательского интерфейса путем модификации CSS файлов и удаления избыточных HTML элементов. Все изменения сохраняют существующую цветовую палитру и функциональность.

## Tasks

- [ ] 1. Создать резервные копии файлов
  - Создать backup копии всех CSS и HTML файлов перед внесением изменений
  - Это позволит легко откатить изменения, если пользователю не понравится результат
  - _Requirements: 13.1, 13.2_

- [ ] 2. Обновить глобальные переменные в main.css
  - [ ] 2.1 Уменьшить spacing variables на 25-30%
    - Изменить --spacing-xs с 4px на 3px
    - Изменить --spacing-sm с 8px на 6px
    - Изменить --spacing-md с 16px на 12px
    - Изменить --spacing-lg с 24px на 18px
    - Изменить --spacing-xl с 32px на 24px
    - _Requirements: 11.1, 11.5_
  
  - [ ] 2.2 Упростить shadow variables
    - Изменить --shadow-sm на более легкую тень
    - Изменить --shadow-md на более легкую тень
    - Изменить --shadow-lg на более легкую тень
    - _Requirements: 12.1, 12.2_

- [ ] 3. Упростить header и navigation в main.css
  - [ ] 3.1 Уменьшить padding в header
    - Изменить padding с var(--spacing-md) var(--spacing-xl) на var(--spacing-sm) var(--spacing-lg)
    - Изменить box-shadow с var(--shadow-md) на var(--shadow-sm)
    - _Requirements: 2.2, 11.2_
  
  - [ ] 3.2 Упростить logo
    - Изменить font-size с var(--font-size-xl) на var(--font-size-lg)
    - _Requirements: 2.4_
  
  - [ ] 3.3 Упростить nav-link
    - Изменить padding с var(--spacing-sm) var(--spacing-md) на var(--spacing-xs) var(--spacing-md)
    - _Requirements: 2.1, 2.2_

- [ ] 4. Упростить cards в main.css
  - [ ] 4.1 Уменьшить padding и тени карточек
    - Изменить padding с var(--spacing-lg) на var(--spacing-md)
    - Изменить box-shadow с var(--shadow-md) на var(--shadow-sm)
    - Добавить border: 1px solid var(--color-border)
    - _Requirements: 3.4, 12.1_
  
  - [ ] 4.2 Упростить hover эффекты карточек
    - Убрать transform: translateY(-4px)
    - Изменить box-shadow с var(--shadow-lg) на var(--shadow-sm)
    - _Requirements: 3.3, 12.2, 12.3_

- [ ] 5. Упростить buttons в components.css
  - [ ] 5.1 Уменьшить padding кнопок
    - Изменить padding с var(--spacing-sm) var(--spacing-lg) на var(--spacing-xs) var(--spacing-md)
    - _Requirements: 6.1_
  
  - [ ] 5.2 Упростить hover эффекты кнопок
    - Убрать transform: translateY(-2px) из всех btn-*:hover
    - Убрать box-shadow из всех btn-*:hover
    - _Requirements: 6.2, 6.3_

- [ ] 6. Упростить forms в components.css
  - [ ] 6.1 Уменьшить padding в search-bar
    - Изменить padding с var(--spacing-lg) на var(--spacing-md)
    - Убрать box-shadow, добавить border: 1px solid var(--color-border)
    - _Requirements: 4.1, 4.3_
  
  - [ ] 6.2 Упростить form-group
    - Изменить margin-bottom с var(--spacing-md) на var(--spacing-sm)
    - _Requirements: 4.2_
  
  - [ ] 6.3 Уменьшить padding в form inputs
    - Изменить padding с var(--spacing-sm) var(--spacing-md) на var(--spacing-xs) var(--spacing-sm)
    - _Requirements: 4.1_

- [ ] 7. Упростить tables в components.css
  - [ ] 7.1 Упростить table-container
    - Изменить padding с var(--spacing-lg) на var(--spacing-md)
    - Убрать box-shadow, добавить border: 1px solid var(--color-border)
    - _Requirements: 5.1, 12.1_
  
  - [ ] 7.2 Уменьшить padding в table cells
    - Изменить padding в thead th с var(--spacing-md) на var(--spacing-sm)
    - Изменить padding в tbody td с var(--spacing-md) на var(--spacing-sm)
    - _Requirements: 5.1_
  
  - [ ] 7.3 Упростить table borders и backgrounds
    - Изменить border-bottom в thead th с 2px на 1px
    - Убрать background-color из tbody tr:nth-child(even)
    - _Requirements: 5.2, 5.3_

- [ ] 8. Упростить modals в components.css
  - [ ] 8.1 Уменьшить padding модальных окон
    - Изменить padding с var(--spacing-xl) на var(--spacing-lg)
    - _Requirements: 7.1_
  
  - [ ] 8.2 Упростить тени и анимации модальных окон
    - Изменить box-shadow с var(--shadow-lg) на var(--shadow-md)
    - Убрать animation: slideUp из .modal-content
    - Убрать animation: fadeIn из .modal
    - _Requirements: 7.2, 7.3_

- [ ] 9. Упростить badges в components.css
  - [ ] 9.1 Уменьшить padding бейджей
    - Изменить padding с var(--spacing-xs) var(--spacing-sm) на 2px var(--spacing-xs)
    - _Requirements: 9.2_

- [ ] 10. Упростить page-specific styles в pages.css
  - [ ] 10.1 Упростить info grids
    - Изменить gap в .ml-info-grid с var(--spacing-md) на var(--spacing-sm)
    - Убрать background-color из .info-grid .info-item
    - Изменить padding в .info-grid .info-item с var(--spacing-md) на var(--spacing-xs)
    - _Requirements: 8.1, 8.2, 8.3_
  
  - [ ] 10.2 Упростить status badges
    - Убрать border из всех .status-badge.*
    - _Requirements: 9.3_
  
  - [ ] 10.3 Уменьшить padding в main-content
    - Изменить padding с var(--spacing-xl) на var(--spacing-lg)
    - _Requirements: 11.2_

- [ ] 11. Удалить footer из всех HTML файлов
  - [ ] 11.1 Удалить footer из index.html
    - Удалить элемент <footer class="footer">...</footer>
    - _Requirements: 10.1_
  
  - [ ] 11.2 Удалить footer из ml-closure.html
    - Удалить элемент <footer class="footer">...</footer>
    - _Requirements: 10.1_
  
  - [ ] 11.3 Удалить footer из prem-koef.html
    - Удалить элемент <footer class="footer">...</footer>
    - _Requirements: 10.1_
  
  - [ ] 11.4 Удалить footer из dce-catalog.html
    - Удалить элемент <footer class="footer">...</footer>
    - _Requirements: 10.1_

- [ ] 12. Удалить user-info из header во всех HTML файлах
  - [ ] 12.1 Удалить user-info из index.html
    - Удалить элемент <div class="user-info">...</div>
    - _Requirements: 2.3_
  
  - [ ] 12.2 Удалить user-info из ml-closure.html
    - Удалить элемент <div class="user-info">...</div>
    - _Requirements: 2.3_
  
  - [ ] 12.3 Удалить user-info из prem-koef.html
    - Удалить элемент <div class="user-info">...</div>
    - _Requirements: 2.3_
  
  - [ ] 12.4 Удалить user-info из dce-catalog.html
    - Удалить элемент <div class="user-info">...</div>
    - _Requirements: 2.3_

- [ ] 13. Упростить карточки на главной странице (index.html)
  - [ ] 13.1 Удалить иконки из карточек
    - Удалить элементы <div class="card-icon">...</div>
    - _Requirements: 3.1, 10.3_
  
  - [ ] 13.2 Удалить футеры карточек с бейджами
    - Удалить элементы <div class="card-footer">...</div>
    - _Requirements: 3.2, 10.3_

- [ ] 14. Удалить page descriptions из страниц
  - [ ] 14.1 Удалить description из ml-closure.html
    - Удалить элемент <p class="page-description">...</p>
    - _Requirements: 10.2_
  
  - [ ] 14.2 Удалить description из prem-koef.html
    - Удалить элемент <p class="page-description">...</p>
    - _Requirements: 10.2_
  
  - [ ] 14.3 Удалить description из dce-catalog.html
    - Удалить элемент <p class="page-description">...</p>
    - _Requirements: 10.2_
  
  - [ ] 14.4 Удалить subtitle из welcome section в index.html
    - Удалить элемент <p class="subtitle">...</p>
    - _Requirements: 10.5_

- [ ] 15. Checkpoint - Проверка изменений
  - Открыть приложение в браузере
  - Проверить все страницы визуально
  - Убедиться, что цветовая палитра не изменилась
  - Проверить, что все функции работают
  - Спросить пользователя о результате

- [ ] 16. Тестирование функциональности
  - [ ] 16.1 Протестировать навигацию
    - Проверить переходы между всеми страницами
    - Убедиться, что активная ссылка подсвечивается
    - _Requirements: 13.1, 13.2_
  
  - [ ] 16.2 Протестировать формы
    - Проверить работу форм поиска на всех страницах
    - Убедиться, что все поля ввода работают
    - Проверить отправку форм
    - _Requirements: 13.3, 4.4_
  
  - [ ] 16.3 Протестировать таблицы
    - Проверить отображение данных в таблицах
    - Убедиться, что hover эффекты работают
    - Проверить интерактивность таблиц
    - _Requirements: 13.1, 5.4_
  
  - [ ] 16.4 Протестировать модальные окна
    - Открыть все модальные окна
    - Проверить закрытие модальных окон
    - Убедиться, что формы в модальных окнах работают
    - _Requirements: 13.1, 7.4_
  
  - [ ] 16.5 Протестировать кнопки
    - Проверить все кнопки на всех страницах
    - Убедиться, что hover эффекты работают
    - Проверить выполнение действий при нажатии
    - _Requirements: 13.1, 13.2_

- [ ] 17. Тестирование адаптивности
  - [ ] 17.1 Протестировать на мобильном разрешении (375px)
    - Проверить все страницы
    - Убедиться, что layout адаптируется
    - Проверить навигацию
    - _Requirements: 14.2, 14.3_
  
  - [ ] 17.2 Протестировать на планшетном разрешении (768px)
    - Проверить все страницы
    - Убедиться, что layout адаптируется
    - _Requirements: 14.2_
  
  - [ ] 17.3 Протестировать на десктопном разрешении (1920px)
    - Проверить все страницы
    - Убедиться, что layout выглядит корректно
    - _Requirements: 14.1_

- [ ] 18. Final checkpoint - Финальная проверка
  - Проверить все страницы в разных браузерах (Chrome, Firefox, Edge)
  - Убедиться, что нет визуальных багов
  - Проверить, что все функции работают корректно
  - Спросить пользователя о финальном результате
  - Если пользователь не доволен - откатить изменения из backup

## Notes

- Все изменения сохраняют существующую цветовую палитру
- Функциональность приложения не изменяется
- Создаются backup копии для возможности отката
- Изменения тестируются на каждом этапе
- Пользователь может запросить откат в любой момент
