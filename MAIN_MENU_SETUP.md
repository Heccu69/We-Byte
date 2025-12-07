# Настройка главного меню игры

## 📋 Обзор
Главное меню с тремя кнопками: **Играть**, **Сохранения**, **Настройки**.

---

## 🎬 Шаг 1: Создание сцены меню

### 1.1. Создать новую сцену:
```
File → New Scene
Сохранить как: "MainMenu" (Assets/Scenes/MainMenu.unity)
```

### 1.2. Убедиться что есть игровая сцена:
```
Ваша текущая игровая сцена должна называться "GameScene"
(или измените имя в скрипте MainMenu.cs, строка 42)
```

---

## 🖼️ Шаг 2: Создание UI главного меню

### 2.1. Создать Canvas:
```
Hierarchy → ПКМ → UI → Canvas
Назвать: "MenuCanvas"

Canvas:
  - Render Mode: Screen Space - Overlay
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: 1920x1080
```

### 2.2. Создать фон (опционально):
```
MenuCanvas → ПКМ → UI → Image
Назвать: "Background"

Image:
  - Color: темный цвет (например, #2C3E50)
  - Stretch: заполнить весь экран (Anchor: stretch-stretch)
```

---

## 📦 Шаг 3: Создание панели главного меню

### 3.1. Создать MenuPanel:
```
MenuCanvas → ПКМ → UI → Panel
Назвать: "MenuPanel"

Rect Transform:
  - Anchor: Center
  - Width: 400
  - Height: 500
  - Pos X: 0, Pos Y: 0

Image (Panel):
  - Color: полупрозрачный (#000000, Alpha: 180)
```

### 3.2. Создать заголовок:
```
MenuPanel → ПКМ → UI → Text - TextMeshPro
Назвать: "TitleText"

Rect Transform:
  - Anchor: Top Center
  - Pos X: 0, Pos Y: -50
  - Width: 350, Height: 80

TextMeshPro:
  - Text: "МОЯ ИГРА"
  - Font Size: 48
  - Alignment: Center
  - Color: белый или яркий цвет
```

### 3.3. Создать кнопку "Играть":
```
MenuPanel → ПКМ → UI → Button - TextMeshPro
Назвать: "PlayButton"

Rect Transform:
  - Anchor: Middle Center
  - Pos X: 0, Pos Y: 50
  - Width: 300, Height: 70

Button:
  - Transition: Color Tint
  - Normal Color: зеленый (#2ECC71)
  - Highlighted Color: светло-зеленый (#58D68D)
  - Pressed Color: темно-зеленый (#27AE60)

Text (внутри кнопки):
  - Text: "ИГРАТЬ"
  - Font Size: 32
  - Alignment: Center
  - Color: белый
```

### 3.4. Создать кнопку "Сохранения":
```
MenuPanel → ПКМ → UI → Button - TextMeshPro
Назвать: "SavesButton"

Rect Transform:
  - Anchor: Middle Center
  - Pos X: 0, Pos Y: -40
  - Width: 300, Height: 70

Button:
  - Normal Color: синий (#3498DB)
  - Highlighted Color: светло-синий (#5DADE2)
  - Pressed Color: темно-синий (#2874A6)

Text:
  - Text: "СОХРАНЕНИЯ"
  - Font Size: 32
```

### 3.5. Создать кнопку "Настройки":
```
MenuPanel → ПКМ → UI → Button - TextMeshPro
Назвать: "SettingsButton"

Rect Transform:
  - Anchor: Middle Center
  - Pos X: 0, Pos Y: -130
  - Width: 300, Height: 70

Button:
  - Normal Color: оранжевый (#E67E22)
  - Highlighted Color: светло-оранжевый (#EB984E)
  - Pressed Color: темно-оранжевый (#CA6F1E)

Text:
  - Text: "НАСТРОЙКИ"
  - Font Size: 32
```

### 3.6. Создать кнопку "Выход" (опционально):
```
MenuPanel → ПКМ → UI → Button - TextMeshPro
Назвать: "ExitButton"

Rect Transform:
  - Anchor: Bottom Center
  - Pos X: 0, Pos Y: 30
  - Width: 200, Height: 50

Button:
  - Normal Color: красный (#E74C3C)

Text:
  - Text: "ВЫХОД"
  - Font Size: 24
```

---

## 📦 Шаг 4: Создание панели сохранений

### 4.1. Создать SavesPanel:
```
MenuCanvas → ПКМ → UI → Panel
Назвать: "SavesPanel"

Rect Transform:
  - Anchor: Center
  - Width: 600
  - Height: 500

Image:
  - Color: полупрозрачный (#000000, Alpha: 180)
```

### 4.2. Добавить заголовок:
```
SavesPanel → ПКМ → UI → Text - TextMeshPro
Назвать: "SavesTitleText"

TextMeshPro:
  - Text: "СОХРАНЕНИЯ"
  - Font Size: 36
  - Alignment: Center
  - Pos Y: -50
```

### 4.3. Добавить текст-заглушку:
```
SavesPanel → ПКМ → UI → Text - TextMeshPro
Назвать: "SavesInfoText"

TextMeshPro:
  - Text: "Здесь будут ваши сохранения"
  - Font Size: 24
  - Alignment: Center
  - Color: серый
```

### 4.4. Кнопка "Назад":
```
SavesPanel → ПКМ → UI → Button - TextMeshPro
Назвать: "BackButton"

Rect Transform:
  - Anchor: Bottom Center
  - Pos Y: 30
  - Width: 200, Height: 60

Text:
  - Text: "НАЗАД"
  - Font Size: 28
```

---

## 📦 Шаг 5: Создание панели настроек

### 5.1. Создать SettingsPanel:
```
MenuCanvas → ПКМ → UI → Panel
Назвать: "SettingsPanel"

Rect Transform:
  - Anchor: Center
  - Width: 600
  - Height: 500

Image:
  - Color: полупрозрачный (#000000, Alpha: 180)
```

### 5.2. Добавить заголовок:
```
SettingsPanel → ПКМ → UI → Text - TextMeshPro
Назвать: "SettingsTitleText"

TextMeshPro:
  - Text: "НАСТРОЙКИ"
  - Font Size: 36
  - Alignment: Center
  - Pos Y: -50
```

### 5.3. Добавить текст-заглушку:
```
SettingsPanel → ПКМ → UI → Text - TextMeshPro
Назвать: "SettingsInfoText"

TextMeshPro:
  - Text: "Здесь будут настройки игры"
  - Font Size: 24
  - Alignment: Center
  - Color: серый
```

### 5.4. Кнопка "Назад":
```
SettingsPanel → ПКМ → UI → Button - TextMeshPro
Назвать: "BackButton"

Rect Transform:
  - Anchor: Bottom Center
  - Pos Y: 30
  - Width: 200, Height: 60

Text:
  - Text: "НАЗАД"
  - Font Size: 28
```

---

## 🔧 Шаг 6: Настройка скрипта MainMenu

### 6.1. Создать пустой объект:
```
Hierarchy → ПКМ → Create Empty
Назвать: "MenuManager"
```

### 6.2. Добавить скрипт:
```
MenuManager → Add Component → MainMenu
```

### 6.3. Назначить панели:
```
MainMenu (Script):
  UI Панели:
    - Menu Panel ← перетащить MenuPanel
    - Saves Panel ← перетащить SavesPanel
    - Settings Panel ← перетащить SettingsPanel
```

---

## 🔗 Шаг 7: Привязка кнопок

### 7.1. Кнопка "Играть":
```
PlayButton → Button (Script) → OnClick():
  1. Нажать "+"
  2. Перетащить MenuManager из Hierarchy
  3. Выбрать: MainMenu → OnPlayButton()
```

### 7.2. Кнопка "Сохранения":
```
SavesButton → Button → OnClick():
  - MenuManager → MainMenu → OnSavesButton()
```

### 7.3. Кнопка "Настройки":
```
SettingsButton → Button → OnClick():
  - MenuManager → MainMenu → OnSettingsButton()
```

### 7.4. Кнопки "Назад" (обе):
```
BackButton (в SavesPanel) → Button → OnClick():
  - MenuManager → MainMenu → OnBackButton()

BackButton (в SettingsPanel) → Button → OnClick():
  - MenuManager → MainMenu → OnBackButton()
```

### 7.5. Кнопка "Выход" (если есть):
```
ExitButton → Button → OnClick():
  - MenuManager → MainMenu → OnExitButton()
```

---

## 🎮 Шаг 8: Настройка сцен в Build Settings

### 8.1. Добавить сцены:
```
File → Build Settings

Scenes In Build:
  0. MainMenu (сцена меню)
  1. GameScene (ваша игровая сцена)

Если имя вашей игровой сцены другое:
  - Откройте MainMenu.cs
  - Строка 42: измените "GameScene" на имя вашей сцены
```

### 8.2. Установить MainMenu как стартовую:
```
Build Settings → перетащить MainMenu на первое место (индекс 0)
```

---

## ✅ Шаг 9: Проверка

### 9.1. Запустить игру:
```
1. Открыть сцену MainMenu
2. Нажать Play
3. Должно появиться главное меню с 3 кнопками
```

### 9.2. Проверить кнопки:
- **"Играть"** → загружается игровая сцена
- **"Сохранения"** → открывается панель сохранений
- **"Настройки"** → открывается панель настроек
- **"Назад"** → возврат в главное меню
- **"Выход"** → выход из игры

---

## 🎨 Дополнительные улучшения

### Добавить логотип игры:
```
MenuPanel → ПКМ → UI → Image
Назвать: "LogoImage"
Разместить над заголовком
Назначить спрайт логотипа
```

### Добавить анимацию кнопок:
```
Кнопка → Add Component → Animator
Создать анимацию появления/нажатия
```

### Добавить фоновую музыку:
```
MenuManager → Add Component → Audio Source
  - AudioClip: [музыка меню]
  - Play On Awake: ✓
  - Loop: ✓
  - Volume: 0.3
```

---

## 📝 Структура Hierarchy:

```
MainMenu (Scene)
├── MenuCanvas
│   ├── Background (Image)
│   ├── MenuPanel (Panel) ← показывается при запуске
│   │   ├── TitleText
│   │   ├── PlayButton
│   │   ├── SavesButton
│   │   ├── SettingsButton
│   │   └── ExitButton
│   ├── SavesPanel (Panel) ← скрыта
│   │   ├── SavesTitleText
│   │   ├── SavesInfoText
│   │   └── BackButton
│   └── SettingsPanel (Panel) ← скрыта
│       ├── SettingsTitleText
│       ├── SettingsInfoText
│       └── BackButton
└── MenuManager (Empty Object)
    └── MainMenu (Script)
```

---

## 🐛 Решение проблем:

**Кнопка "Играть" не работает:**
- Проверьте что сцена GameScene добавлена в Build Settings
- Проверьте имя сцены в MainMenu.cs (строка 42)

**Панели не переключаются:**
- Проверьте что все панели назначены в MainMenu (Script)
- Проверьте что кнопки правильно привязаны к методам

**Меню не появляется при запуске:**
- Убедитесь что MainMenu - первая сцена в Build Settings
- Проверьте что MenuPanel активна в Inspector

---

## ✅ Готово!

Теперь при запуске игры появится главное меню с тремя кнопками! 🎉
