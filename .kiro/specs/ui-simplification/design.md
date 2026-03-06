# Design Document: Упрощение интерфейса системы ОТК

## Overview

Данный документ описывает дизайн упрощения пользовательского интерфейса веб-приложения системы ОТК. Основная цель - создать более минималистичный, чистый интерфейс с сохранением существующей цветовой палитры и всей функциональности.

## Architecture

### Подход к упрощению

Упрощение будет выполнено путем модификации существующих CSS файлов:
- `main.css` - глобальные стили и layout
- `components.css` - компоненты (кнопки, формы, модальные окна)
- `pages.css` - стили страниц
- HTML файлы - удаление избыточных элементов

### Принципы упрощения

1. **Минимализм**: Убрать все декоративные элементы, которые не несут функциональной нагрузки
2. **Компактность**: Уменьшить отступы и интервалы для более плотного размещения информации
3. **Плоский дизайн**: Минимизировать тени, градиенты и 3D-эффекты
4. **Сохранение цветов**: Не менять существующую цветовую палитру
5. **Функциональность**: Сохранить все существующие функции и интерактивность

## Components and Interfaces

### 1. Глобальные переменные (main.css)

#### Изменения в spacing variables

```css
/* Было */
--spacing-xs: 4px;
--spacing-sm: 8px;
--spacing-md: 16px;
--spacing-lg: 24px;
--spacing-xl: 32px;

/* Станет (уменьшение на 25-30%) */
--spacing-xs: 3px;
--spacing-sm: 6px;
--spacing-md: 12px;
--spacing-lg: 18px;
--spacing-xl: 24px;
```

#### Изменения в shadows

```css
/* Было */
--shadow-sm: 0 2px 4px var(--color-shadow);
--shadow-md: 0 4px 8px var(--color-shadow);
--shadow-lg: 0 8px 16px var(--color-shadow);

/* Станет (минимальные тени) */
--shadow-sm: 0 1px 2px rgba(10, 9, 3, 0.1);
--shadow-md: 0 2px 4px rgba(10, 9, 3, 0.15);
--shadow-lg: 0 3px 6px rgba(10, 9, 3, 0.2);
```

### 2. Header (Шапка)

#### Упрощения:
- Убрать блок user-info (информация о пользователе и кнопка выхода)
- Уменьшить padding шапки
- Упростить навигацию

```css
.header {
    padding: var(--spacing-sm) var(--spacing-lg); /* было: var(--spacing-md) var(--spacing-xl) */
    box-shadow: var(--shadow-sm); /* было: var(--shadow-md) */
}

.logo {
    font-size: var(--font-size-lg); /* было: var(--font-size-xl) */
}

.nav-link {
    padding: var(--spacing-xs) var(--spacing-md); /* было: var(--spacing-sm) var(--spacing-md) */
}
```

### 3. Cards (Карточки)

#### Упрощения:
- Убрать иконки карточек
- Убрать футеры с бейджами
- Уменьшить padding
- Упростить hover эффекты
- Минимизировать тени

```css
.card {
    padding: var(--spacing-md); /* было: var(--spacing-lg) */
    box-shadow: var(--shadow-sm); /* было: var(--shadow-md) */
    border: 1px solid var(--color-border); /* добавить простую границу */
}

.card-clickable:hover {
    transform: none; /* было: translateY(-4px) */
    box-shadow: var(--shadow-sm); /* было: var(--shadow-lg) */
    border-color: var(--color-primary);
}
```

### 4. Buttons (Кнопки)

#### Упрощения:
- Убрать тени
- Упростить hover эффекты
- Уменьшить padding

```css
.btn {
    padding: var(--spacing-xs) var(--spacing-md); /* было: var(--spacing-sm) var(--spacing-lg) */
    box-shadow: none; /* убрать тени */
}

.btn-primary:hover:not(:disabled) {
    background-color: #f0ed70;
    transform: none; /* было: translateY(-2px) */
    box-shadow: none; /* было: var(--shadow-md) */
}
```

### 5. Forms (Формы)

#### Упрощения:
- Уменьшить padding в search-bar
- Упростить form-group
- Убрать декоративные тени

```css
.search-bar {
    padding: var(--spacing-md); /* было: var(--spacing-lg) */
    box-shadow: none; /* было: var(--shadow-md) */
    border: 1px solid var(--color-border);
}

.form-group {
    margin-bottom: var(--spacing-sm); /* было: var(--spacing-md) */
}

.form-group input,
.form-group select,
.form-group textarea {
    padding: var(--spacing-xs) var(--spacing-sm); /* было: var(--spacing-sm) var(--spacing-md) */
}
```

### 6. Tables (Таблицы)

#### Упрощения:
- Уменьшить padding ячеек
- Упростить границы
- Убрать чередующиеся фоны строк
- Упростить заголовки

```css
.table-container {
    padding: var(--spacing-md); /* было: var(--spacing-lg) */
    box-shadow: none; /* было: var(--shadow-md) */
    border: 1px solid var(--color-border);
}

thead th {
    padding: var(--spacing-sm); /* было: var(--spacing-md) */
    border-bottom: 1px solid var(--color-primary); /* было: 2px */
}

tbody td {
    padding: var(--spacing-sm); /* было: var(--spacing-md) */
}

tbody tr:nth-child(even) {
    background-color: transparent; /* убрать чередование */
}
```

### 7. Modals (Модальные окна)

#### Упрощения:
- Уменьшить padding
- Упростить тени
- Убрать анимации

```css
.modal-content {
    padding: var(--spacing-lg); /* было: var(--spacing-xl) */
    box-shadow: var(--shadow-md); /* было: var(--shadow-lg) */
    animation: none; /* было: slideUp */
}

.modal {
    animation: none; /* было: fadeIn */
}
```

### 8. Badges (Бейджи)

#### Упрощения:
- Уменьшить padding
- Убрать границы у status-badge

```css
.badge {
    padding: 2px var(--spacing-xs); /* было: var(--spacing-xs) var(--spacing-sm) */
}

.status-badge {
    border: none; /* убрать границы */
}
```

### 9. Info Grids (Информационные сетки)

#### Упрощения:
- Уменьшить gap
- Убрать декоративные фоны

```css
.ml-info-grid {
    gap: var(--spacing-sm); /* было: var(--spacing-md) */
}

.info-grid .info-item {
    background-color: transparent; /* было: var(--color-bg-lighter) */
    padding: var(--spacing-xs); /* было: var(--spacing-md) */
}
```

### 10. Main Content

#### Упрощения:
- Уменьшить padding

```css
.main-content {
    padding: var(--spacing-lg); /* было: var(--spacing-xl) */
}
```

## Data Models

Изменения не затрагивают модели данных. Все изменения касаются только представления (CSS и HTML).

## Correctness Properties

*A property is a characteristic or behavior that should hold true across all valid executions of a system-essentially, a formal statement about what the system should do. Properties serve as the bridge between human-readable specifications and machine-verifiable correctness guarantees.*

### Property 1: Color Palette Preservation
*For any* CSS file after modification, all color values should match the original color palette (FFFD82, 0A0903, EFCEFA, C89B7B, 2F3E46, and their derivatives)
**Validates: Requirements 1.1, 1.2, 1.3**

### Property 2: Spacing Reduction Consistency
*For any* spacing variable, the new value should be approximately 70-75% of the original value
**Validates: Requirements 11.1, 11.2, 11.3, 11.4**

### Property 3: Functional Element Preservation
*For any* interactive element (button, link, form input), the element should remain functional and accessible after simplification
**Validates: Requirements 13.1, 13.2, 13.3, 13.5**

### Property 4: Responsive Breakpoint Preservation
*For any* responsive breakpoint in the original CSS, the breakpoint should remain unchanged in the simplified version
**Validates: Requirements 14.1, 14.2, 14.3, 14.4**

### Property 5: Visual Feedback Maintenance
*For any* interactive element, hover and focus states should provide clear visual feedback
**Validates: Requirements 12.4, 13.5**

## Error Handling

### CSS Validation
- Validate all CSS changes for syntax errors
- Ensure no broken selectors or properties
- Test in multiple browsers (Chrome, Firefox, Edge)

### Visual Regression
- Compare before/after screenshots
- Verify no layout breaks
- Check responsive behavior on different screen sizes

### Functionality Testing
- Test all forms and inputs
- Verify all buttons and links work
- Check modal windows open and close correctly
- Validate table sorting and filtering

## Testing Strategy

### Manual Testing

1. **Visual Inspection**
   - Compare each page before and after changes
   - Verify color palette consistency
   - Check spacing and alignment
   - Validate responsive behavior

2. **Functional Testing**
   - Test all interactive elements
   - Verify form submissions
   - Check modal interactions
   - Test navigation between pages

3. **Cross-browser Testing**
   - Test in Chrome, Firefox, Edge
   - Verify consistent appearance
   - Check for browser-specific issues

### Test Cases

#### Test Case 1: Header Simplification
- Navigate to each page
- Verify header displays correctly
- Check navigation links work
- Confirm user info section is removed

#### Test Case 2: Card Simplification
- View main page
- Verify cards display without icons
- Check card footers are removed
- Test card hover effects

#### Test Case 3: Form Simplification
- Open search forms on each page
- Verify reduced padding
- Check form inputs work correctly
- Test form submission

#### Test Case 4: Table Simplification
- View tables on each page
- Verify reduced cell padding
- Check table remains readable
- Test table interactions (sorting, filtering)

#### Test Case 5: Button Simplification
- Test all buttons across pages
- Verify simplified styling
- Check button functionality
- Test hover states

#### Test Case 6: Modal Simplification
- Open modal windows
- Verify reduced padding
- Check modal functionality
- Test close buttons

#### Test Case 7: Responsive Behavior
- Test on mobile viewport (375px)
- Test on tablet viewport (768px)
- Test on desktop viewport (1920px)
- Verify layout adapts correctly

#### Test Case 8: Color Consistency
- Inspect all pages
- Verify no new colors introduced
- Check color palette maintained
- Validate contrast ratios

### Acceptance Criteria

- All pages load without errors
- All interactive elements function correctly
- Color palette remains unchanged
- Spacing is reduced by 25-30%
- No visual regressions
- Responsive design works on all screen sizes
- All forms submit correctly
- All modals open and close properly

## Implementation Notes

### Files to Modify

1. **CSS Files**
   - `WebAPI_OTK/wwwroot/css/main.css`
   - `WebAPI_OTK/wwwroot/css/components.css`
   - `WebAPI_OTK/wwwroot/css/pages.css`

2. **HTML Files**
   - `WebAPI_OTK/wwwroot/index.html` (remove footer, user-info, card icons)
   - `WebAPI_OTK/wwwroot/pages/ml-closure.html` (remove footer, page description)
   - `WebAPI_OTK/wwwroot/pages/prem-koef.html` (remove footer, page description)
   - `WebAPI_OTK/wwwroot/pages/dce-catalog.html` (remove footer, page description)

### Implementation Order

1. Update CSS variables in `main.css`
2. Simplify header and navigation in `main.css`
3. Simplify cards in `main.css`
4. Simplify buttons in `components.css`
5. Simplify forms in `components.css`
6. Simplify tables in `components.css`
7. Simplify modals in `components.css`
8. Simplify badges in `components.css`
9. Simplify page-specific styles in `pages.css`
10. Remove HTML elements (footer, user-info, icons, descriptions)
11. Test all changes
12. Fix any issues

### Rollback Plan

If user is not satisfied with changes:
1. Git revert to previous commit
2. Or restore backup copies of CSS files
3. Restore HTML files from backup

### Browser Compatibility

Target browsers:
- Chrome 90+
- Firefox 88+
- Edge 90+
- Safari 14+

All CSS properties used are widely supported and do not require vendor prefixes.
