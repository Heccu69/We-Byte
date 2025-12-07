# Визуальная инструкция по настройке слотов сохранений

## Структура главного меню

```
Canvas
  └── MainMenu
        ├── SaveButton (существующая кнопка "Сохранения")
        │   └── OnClick() → SavePanel.SetActive(true)
        │
        └── SavePanel (существующая панель)
              ├── Title (Text TMP) - "Выберите сохранение"
              │
              ├── SlotsContainer (Empty GameObject + Vertical Layout Group)
              │     ├── SlotPanel_0
              │     │     ├── InfoContainer (слева)
              │     │     │     ├── SlotName_0 (Text TMP)
              │     │     │     └── SlotInfo_0 (Text TMP)
              │     │     └── ButtonsContainer (справа + Horizontal Layout)
              │     │           ├── PlayButton_0 (Button)
              │     │           └── DeleteButton_0 (Button)
              │     │
              │     ├── SlotPanel_1 (аналогично)
              │     └── SlotPanel_2 (аналогично)
              │
              ├── CreateSavePanel (изначально выключена)
              │     ├── Background (Image)
              │     ├── Title (Text TMP) - "Создать новое сохранение"
              │     ├── SaveNameInput (Input Field TMP)
              │     ├── CreateButton (Button)
              │     └── CancelButton (Button)
              │
              ├── CloseButton (опционально)
              │
              └── SaveSlotsManager (Empty GameObject + компонент)
```

---

## Пошаговая настройка с картинками

### Шаг 1: Найдите существующую панель

**В Hierarchy найдите:**
```
Canvas → MainMenu → SaveButton
Canvas → MainMenu → SavePanel
```

**Если панели нет - создайте:**
1. Правый клик на MainMenu → UI → Panel
2. Назовите "SavePanel"
3. Настройте размер (например, 800x600)
4. Центрируйте на экране

---

### Шаг 2: Создайте контейнер слотов

**Внутри SavePanel:**

1. **Создайте заголовок:**
   ```
   Правый клик на SavePanel → UI → Text - TextMeshPro
   Имя: "Title"
   ```
   - Текст: "Выберите сохранение"
   - Font Size: 36
   - Alignment: Center Top
   - Позиция: верх панели

2. **Создайте контейнер:**
   ```
   Правый клик на SavePanel → Create Empty
   Имя: "SlotsContainer"
   ```
   
   **Настройте Rect Transform:**
   - Anchor: Stretch (растянуть по ширине)
   - Left: 50, Right: 50
   - Top: 100, Bottom: 100
   
   **Добавьте Vertical Layout Group:**
   ```
   Add Component → Vertical Layout Group
   ```
   - Control Child Size: Width ✓, Height ☐
   - Child Force Expand: Width ✓, Height ☐
   - Spacing: 15
   - Padding: Left 10, Right 10, Top 10, Bottom 10

---

### Шаг 3: Создайте первый слот (SlotPanel_0)

**Внутри SlotsContainer:**

1. **Создайте панель слота:**
   ```
   Правый клик на SlotsContainer → UI → Panel
   Имя: "SlotPanel_0"
   ```
   
   **Добавьте Layout Element:**
   ```
   Add Component → Layout Element
   ```
   - Min Height: 100
   - Preferred Height: 100

2. **Создайте левую часть (информация):**
   ```
   Правый клик на SlotPanel_0 → Create Empty
   Имя: "InfoContainer"
   ```
   
   **Rect Transform:**
   - Anchor: Left (якорь слева)
   - Pos X: 20
   - Pos Y: 0
   - Width: 400
   - Height: 80
   
   **Внутри InfoContainer создайте 2 текста:**
   
   a) **Название сохранения:**
   ```
   UI → Text - TextMeshPro
   Имя: "SlotName_0"
   ```
   - Текст: "Пустой слот"
   - Font Size: 24
   - Font Style: Bold
   - Color: белый
   - Alignment: Left Top
   - Pos Y: 10
   
   b) **Информация:**
   ```
   UI → Text - TextMeshPro
   Имя: "SlotInfo_0"
   ```
   - Текст: "Нажмите для создания"
   - Font Size: 16
   - Color: серый (#AAAAAA)
   - Alignment: Left Top
   - Pos Y: -20

3. **Создайте правую часть (кнопки):**
   ```
   Правый клик на SlotPanel_0 → Create Empty
   Имя: "ButtonsContainer"
   ```
   
   **Rect Transform:**
   - Anchor: Right (якорь справа)
   - Pos X: -20
   - Pos Y: 0
   - Width: 200
   - Height: 80
   
   **Добавьте Horizontal Layout Group:**
   ```
   Add Component → Horizontal Layout Group
   ```
   - Spacing: 10
   - Child Alignment: Middle Right
   
   **Внутри ButtonsContainer создайте 2 кнопки:**
   
   a) **Кнопка "Играть":**
   ```
   UI → Button - TextMeshPro
   Имя: "PlayButton_0"
   ```
   - Текст: "Играть"
   - Font Size: 18
   - Color: зеленый (#00FF00)
   - Layout Element: Min Width 100
   
   b) **Кнопка "Удалить":**
   ```
   UI → Button - TextMeshPro
   Имя: "DeleteButton_0"
   ```
   - Текст: "Удалить"
   - Font Size: 18
   - Color: красный (#FF0000)
   - Layout Element: Min Width 80

---

### Шаг 4: Дублируйте слот

1. **Выберите SlotPanel_0 в Hierarchy**
2. **Ctrl+D (дублировать) 2 раза**
3. **Переименуйте:**
   - SlotPanel_0 (1) → SlotPanel_1
   - SlotPanel_0 (2) → SlotPanel_2

4. **Переименуйте дочерние элементы:**
   - В SlotPanel_1: SlotName_0 → SlotName_1, SlotInfo_0 → SlotInfo_1, и т.д.
   - В SlotPanel_2: SlotName_0 → SlotName_2, SlotInfo_0 → SlotInfo_2, и т.д.

---

### Шаг 5: Создайте панель создания сохранения

**Внутри SavePanel:**

1. **Создайте панель:**
   ```
   Правый клик на SavePanel → UI → Panel
   Имя: "CreateSavePanel"
   ```
   
   **Rect Transform:**
   - Anchor: Center
   - Pos X: 0, Pos Y: 0
   - Width: 500
   - Height: 250
   
   **Image:**
   - Color: темно-серый (#333333)
   - Alpha: 230 (почти непрозрачный)

2. **Создайте заголовок:**
   ```
   UI → Text - TextMeshPro
   Имя: "Title"
   ```
   - Текст: "Создать новое сохранение"
   - Font Size: 28
   - Alignment: Center Top
   - Pos Y: -30

3. **Создайте поле ввода:**
   ```
   UI → Input Field - TextMeshPro
   Имя: "SaveNameInput"
   ```
   - Placeholder: "Введите название..."
   - Font Size: 20
   - Pos Y: 0
   - Width: 400, Height: 50

4. **Создайте кнопки:**
   
   a) **Кнопка "Создать":**
   ```
   UI → Button - TextMeshPro
   Имя: "CreateButton"
   ```
   - Текст: "Создать"
   - Pos X: -80, Pos Y: -80
   - Width: 150, Height: 50
   - Color: зеленый
   
   b) **Кнопка "Отмена":**
   ```
   UI → Button - TextMeshPro
   Имя: "CancelButton"
   ```
   - Текст: "Отмена"
   - Pos X: 80, Pos Y: -80
   - Width: 150, Height: 50
   - Color: серый

5. **ВАЖНО: Отключите панель!**
   - Снимите галочку слева от "CreateSavePanel" в Hierarchy

---

### Шаг 6: Настройте SaveSlotsManager

1. **Создайте менеджер:**
   ```
   Правый клик на SavePanel → Create Empty
   Имя: "SaveSlotsManager"
   ```

2. **Добавьте компонент:**
   ```
   Add Component → Save Slots Manager
   ```

3. **Заполните массивы в Inspector:**

   **Slot Panels (Size: 3):**
   - Перетащите SlotPanel_0, SlotPanel_1, SlotPanel_2
   
   **Slot Names (Size: 3):**
   - Перетащите SlotName_0, SlotName_1, SlotName_2
   
   **Slot Info Texts (Size: 3):**
   - Перетащите SlotInfo_0, SlotInfo_1, SlotInfo_2
   
   **Play Buttons (Size: 3):**
   - Перетащите PlayButton_0, PlayButton_1, PlayButton_2
   
   **Delete Buttons (Size: 3):**
   - Перетащите DeleteButton_0, DeleteButton_1, DeleteButton_2
   
   **Create Save Panel:**
   - Перетащите CreateSavePanel
   
   **Save Name Input:**
   - Перетащите SaveNameInput
   
   **Create Button:**
   - Перетащите CreateButton
   
   **Cancel Button:**
   - Перетащите CancelButton
   
   **Game Scene Name:**
   - Введите название вашей игровой сцены (например, "GameScene")

---

### Шаг 7: Проверьте кнопку открытия

1. **Найдите кнопку "Сохранения" в меню**
2. **Выберите её в Hierarchy**
3. **В Inspector найдите OnClick()**
4. **Проверьте или добавьте:**
   ```
   OnClick()
     Runtime Only
     SavePanel → GameObject.SetActive
     ✓ (галочка)
   ```

---

### Шаг 8: Скройте панель по умолчанию

1. **Выберите SavePanel в Hierarchy**
2. **Снимите галочку слева от имени**
3. **Сохраните сцену (Ctrl+S)**

---

## Игровая сцена - Уведомления

### Создайте текст уведомления:

1. **В игровой сцене:**
   ```
   Canvas → UI → Text - TextMeshPro
   Имя: "SaveNotificationText"
   ```

2. **Настройте Rect Transform:**
   - Anchor Preset: **Top Right** (нажмите с Shift+Alt)
   - Pos X: -20
   - Pos Y: -20
   - Width: 300
   - Height: 50

3. **Настройте текст:**
   - Текст: "" (оставьте пустым!)
   - Font Size: 24
   - Font Style: Bold
   - Color: зеленый (#00FF00)
   - Alignment: Right (→)

4. **Добавьте тень:**
   ```
   Add Component → Shadow
   ```
   - Effect Distance: X=2, Y=-2
   - Color: черный (#000000)

5. **ВАЖНО: Оставьте активным!**
   - ✓ Галочка должна быть включена

### Настройте InGameSaveManager:

1. **Создайте объект:**
   ```
   Create Empty
   Имя: "InGameSaveManager"
   ```

2. **Добавьте компонент:**
   ```
   Add Component → In Game Save Manager
   ```

3. **Настройте:**
   - Save Key: F5
   - Auto Save Enabled: ✓
   - Auto Save Interval: 300
   - Save Notification Text: перетащите SaveNotificationText
   - Notification Duration: 2

---

## Проверка работы

### В главном меню:

1. **Запустите игру**
2. **Нажмите кнопку "Сохранения"**
3. **Должна открыться панель с 3 слотами**
4. **Нажмите на пустой слот**
5. **Должна открыться панель создания**
6. **Введите название и нажмите "Создать"**
7. **Игра должна загрузиться**

### В игре:

1. **Нажмите F5**
2. **В правом верхнем углу появится "Игра сохранена"**
3. **Через 2 секунды текст исчезнет**
4. **Объект текста остается активным всегда**

---

## Готово! ✨

Все 3 слота находятся внутри одной панели, которая открывается по кнопке.
Текст уведомления всегда активен, просто меняется содержимое.
