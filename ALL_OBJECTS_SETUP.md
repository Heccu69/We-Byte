# ПОЛНАЯ НАСТРОЙКА ВСЕХ ОБЪЕКТОВ В ПРОЕКТЕ

## 📋 СПИСОК ВСЕХ СКРИПТОВ И ОБЪЕКТОВ

### Система заказов (Order System)
1. **ScoreManager** - управление очками
2. **OrderSystem** - управление заказами
3. **OrderDisplay** - отображение заказа на PC
4. **UnderCakePlate** - тарелка для проверки торта

### Игрок (Player)
5. **PlayerMove** - движение игрока
6. **PlayerCatch** - подбор объектов

### Конвейер (Conveyor)
7. **ConveyorSpawner** - спавнер коржей и платформ
8. **VerticalConveyor** - движение конвейера
9. **ConveyorPlatform** - платформа конвейера
10. **ConveyorPairLink** - связь платформы и коржа

### Объекты
11. **PickupObject** - подбираемые объекты (коржи)

### Камера
12. **CameraFollow** - следование за игроком
13. **CameraSkyboxFix** - фикс скайбокса

### Погода и свет
14. **WeatherController** - контроллер погоды
15. **WindowLightEffect** - эффект света из окна

### Прочее
16. **EnemyMove** - движение врагов
17. **LocalSelector** - выбор локали

---

# 🎯 ПОШАГОВАЯ НАСТРОЙКА ВСЕХ ОБЪЕКТОВ

---

## 1️⃣ СИСТЕМА ЗАКАЗОВ

### ScoreManager (управление очками)

**Создание:**
```
1. Hierarchy → ПКМ → Create Empty
2. Назвать: "ScoreManager"
3. Inspector → Add Component → ScoreManager
```

**Настройка:**
```
ScoreManager (Script):
  - Score Text: ScoreText (создадим позже)
```

**Где находится:** В корне Hierarchy

---

### OrderSystem (управление заказами)

**Создание:**
```
1. Hierarchy → ПКМ → Create Empty
2. Назвать: "OrderSystem"
3. Inspector → Add Component → OrderSystem
```

**Настройка:**
```
OrderSystem (Script):
  - Min Korzh Count: 2
  - Max Korzh Count: 5
```

**Где находится:** В корне Hierarchy

---

### PC с OrderDisplay (отображение заказа)

**Вариант A: Если PC уже есть**
```
1. Найти объект "PC" в Hierarchy
2. Inspector → Add Component → OrderDisplay
```

**Вариант B: Создать новый PC**
```
1. Hierarchy → ПКМ → 2D Object → Sprite
2. Назвать: "PC"
3. Sprite Renderer → Sprite: выбрать спрайт компьютера
4. Inspector → Add Component → OrderDisplay
5. Position: (5, 0, 0) или где-то на столе
```

**Настройка:**
```
OrderDisplay (Script):
  - Order Text: OrderText (создадим позже)
  - Order Panel: None (опционально)
```

**Где находится:** В сцене (на столе)

---

### UnderCake с UnderCakePlate (тарелка для торта)

**Вариант A: Если UnderCake уже есть**
```
1. Найти объект "UnderCake" в Hierarchy
2. Inspector → Add Component → Circle Collider 2D
   - Is Trigger: true
   - Radius: 1.5
3. Inspector → Add Component → UnderCakePlate
```

**Вариант B: Создать новый UnderCake**
```
1. Hierarchy → ПКМ → 2D Object → Sprite
2. Назвать: "UnderCake"
3. Sprite Renderer → Sprite: выбрать спрайт тарелки
4. Add Component → Circle Collider 2D
   - Is Trigger: true
   - Radius: 1.5
5. Add Component → UnderCakePlate
6. Position: (0, -2, 0) или где игрок будет складывать коржи
```

**Настройка:**
```
Circle Collider 2D:
  - Is Trigger: ✓
  - Radius: 1.5

UnderCakePlate (Script):
  - Check Radius: 2
  - Stack Tolerance: 0.3
  - Show Debug Info: ✓
```

**Где находится:** В сцене (на полу/столе)

---

## 2️⃣ ИГРОК (PLAYER)

### Player с PlayerMove и PlayerCatch

**Вариант A: Если Player уже есть**
```
1. Найти объект "Player" в Hierarchy
2. Проверить компоненты:
   - Sprite Renderer ✓
   - Rigidbody2D ✓
   - Collider2D ✓
   - PlayerMove ✓
   - PlayerCatch ✓
   - Animator (опционально)
```

**Вариант B: Создать нового Player**
```
1. Hierarchy → ПКМ → 2D Object → Sprite
2. Назвать: "Player"
3. Sprite Renderer → Sprite: выбрать спрайт игрока
4. Add Component → Rigidbody2D
   - Body Type: Dynamic
   - Gravity Scale: 0
   - Constraints: Freeze Rotation Z
5. Add Component → Box Collider 2D
   - Is Trigger: false
6. Add Component → PlayerMove
7. Add Component → PlayerCatch
8. Add Component → Animator (опционально)
9. Position: (0, 0, 0) - стартовая позиция
```

**Настройка PlayerMove:**
```
PlayerMove (Script):
  - Speed: 5
  - Animator: перетащить Animator (если есть)
  - Rgb: перетащить Rigidbody2D
```

**Настройка PlayerCatch:**
```
PlayerCatch (Script):
  - Pickup Range: 1.5
  - Pickup Layer: Default (или создать слой "Pickup")
  - Hand Transform: None (создастся автоматически)
  - Hand Offset: (0.5, 0.3, 0)
```

**Создать Hand (рука игрока):**
```
1. Player → ПКМ → Create Empty
2. Назвать: "Hand"
3. Position: (0.5, 0.3, 0)
4. Перетащить Hand в поле Hand Transform компонента PlayerCatch
```

**Где находится:** В сцене (управляемый игроком)

---

## 3️⃣ КОНВЕЙЕР (CONVEYOR)

### ConveyorSpawner (спавнер коржей)

**Создание:**
```
1. Hierarchy → ПКМ → Create Empty
2. Назвать: "ConveyorSpawner"
3. Inspector → Add Component → ConveyorSpawner
4. Position: где будут появляться коржи (например, 0, 5, 0)
```

**Настройка:**
```
ConveyorSpawner (Script):
  
  Настройки спавна:
  - Korzh Prefab: перетащить префаб Korzh (если есть)
  - Spawn Interval: 2
  - Spawn Offset Y: 0
  
  Спрайты коржей:
  - Korzh Sprites: Size = 3-5
    - Element 0: спрайт коржа 1
    - Element 1: спрайт коржа 2
    - Element 2: спрайт коржа 3
  
  Платформа конвейера:
  - Platform Prefab: None (создается автоматически)
  - Platform Sprite: спрайт платформы
  - Platform Offset: (0, -0.15, 0)
  
  Ограничения:
  - Max Active Pairs: 5
  - Max Platforms In Scene: 5
  - Max Korzhs In Scene: 25
```

**Где находится:** В сцене (точка спавна)

---

### VerticalConveyor (движение конвейера)

**Создание:**
```
1. Hierarchy → ПКМ → 2D Object → Sprite
2. Назвать: "VerticalConveyor"
3. Sprite Renderer → Sprite: спрайт конвейера (длинный)
4. Add Component → Box Collider 2D
   - Is Trigger: true
   - Size: подобрать по размеру конвейера
5. Add Component → VerticalConveyor
6. Position: (0, 0, 0) - где конвейер в сцене
```

**Настройка:**
```
Box Collider 2D:
  - Is Trigger: ✓
  - Size: (1, 10) - по размеру конвейера

VerticalConveyor (Script):
  - Move Speed: 2
  - Move Up: ✓ (или false для движения вниз)
  - Top Bound: 5 (верхняя граница)
  - Bottom Bound: -5 (нижняя граница)
```

**Где находится:** В сцене (вертикальный конвейер)

---

### Префабы (Prefabs)

#### Korzh Prefab (корж)

**Создание:**
```
1. Hierarchy → ПКМ → 2D Object → Sprite
2. Назвать: "Korzh"
3. Sprite Renderer → Sprite: спрайт коржа
4. Add Component → Rigidbody2D
   - Body Type: Dynamic
   - Mass: 1.5
   - Linear Drag: 0.5
   - Angular Drag: 0.5
   - Gravity Scale: 1
   - Constraints: Freeze Rotation Z
   - Collision Detection: Continuous
5. Add Component → Box Collider 2D
   - Is Trigger: false
   - Size: (3, 0.8)
6. Add Component → PickupObject
7. Tag: "ConveyorObject"
8. Перетащить в папку Prefabs
9. Удалить из сцены
```

**Настройка PickupObject:**
```
PickupObject (Script):
  - Object Type: Korzh
  - Object Name: "Корж"
```

---

#### Platform Prefab (платформа) - ОПЦИОНАЛЬНО

```
Создается автоматически ConveyorSpawner
Можно не создавать вручную
```

---

## 4️⃣ КАМЕРА

### Main Camera с CameraFollow

**Настройка:**
```
1. Найти "Main Camera" в Hierarchy
2. Inspector → Add Component → CameraFollow
3. Если нужен фикс скайбокса:
   Add Component → CameraSkyboxFix
```

**Настройка CameraFollow:**
```
CameraFollow (Script):
  
  Настройки следования:
  - Target: перетащить Player
  - Offset: (0, 0, -10)
  - Smooth Speed: 0.125
  
  Ограничения движения:
  - Use Bounds: false (или true если нужны границы)
  - Min Bounds: (-10, -10, -10)
  - Max Bounds: (10, 10, -10)
```

**Где находится:** В корне Hierarchy (Main Camera)

---

## 5️⃣ UI ИНТЕРФЕЙС

### Canvas

**Создание (если нет):**
```
1. Hierarchy → ПКМ → UI → Canvas
2. Canvas автоматически создастся с EventSystem
```

**Настройка:**
```
Canvas:
  - Render Mode: Screen Space - Overlay
  
Canvas Scaler:
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: 1920 x 1080
  - Match: 0.5
```

---

### ScoreText (отображение очков)

**Создание:**
```
1. Canvas → ПКМ → UI → Text - TextMeshPro
2. Назвать: "ScoreText"
```

**Настройка:**
```
Rect Transform:
  - Anchor Presets: Top Center
  - Pos X: 0
  - Pos Y: -50
  - Width: 300
  - Height: 60

TextMeshPro - Text:
  - Text: "Очки: 0"
  - Font Size: 36
  - Alignment: Center
  - Color: белый
```

**Связать с ScoreManager:**
```
ScoreManager → Score Text ← перетащить ScoreText
```

---

### OrderText (отображение заказа)

**Создание:**
```
1. Canvas → ПКМ → UI → Text - TextMeshPro
2. Назвать: "OrderText"
```

**Настройка:**
```
Rect Transform:
  - Anchor Presets: Top Left
  - Pos X: 150
  - Pos Y: -100
  - Width: 250
  - Height: 100

TextMeshPro - Text:
  - Text: "ЗАКАЗ:\n3 коржей"
  - Font Size: 32
  - Alignment: Center
  - Color: желтый (#FFFF00)
```

**Связать с PC:**
```
PC → OrderDisplay → Order Text ← перетащить OrderText
```

---

### CheckOrderButton (кнопка проверки)

**Создание:**
```
1. Canvas → ПКМ → UI → Button - TextMeshPro
2. Назвать: "CheckOrderButton"
```

**Настройка:**
```
Rect Transform:
  - Anchor Presets: Bottom Center
  - Pos X: 0
  - Pos Y: 100
  - Width: 300
  - Height: 80

Button (Script):
  - Interactable: ✓
  - Transition: Color Tint
  - Normal Color: зеленый (0, 200, 0)
  - Highlighted Color: светло-зеленый (100, 255, 100)
  - Pressed Color: темно-зеленый (0, 150, 0)

Text (TMP) - дочерний объект:
  - Text: "Проверить заказ"
  - Font Size: 28
  - Alignment: Center
  - Color: белый
```

**Привязать к UnderCake:**
```
1. CheckOrderButton → Button (Script)
2. On Click () → "+"
3. Перетащить UnderCake
4. UnderCakePlate → CheckOrderButton ()
```

---

## 6️⃣ ПОГОДА И СВЕТ (ОПЦИОНАЛЬНО)

### WeatherController

**Создание:**
```
1. Hierarchy → ПКМ → Create Empty
2. Назвать: "WeatherController"
3. Inspector → Add Component → WeatherController
```

**Настройка:**
```
WeatherController (Script):
  - Настроить по необходимости
```

---

### WindowLight с WindowLightEffect

**Создание:**
```
1. Hierarchy → ПКМ → Light → 2D → Point Light 2D
2. Назвать: "WindowLight"
3. Inspector → Add Component → WindowLightEffect
```

**Настройка:**
```
Light 2D:
  - Intensity: 1
  - Color: теплый желтый

WindowLightEffect (Script):
  - Base Intensity: 1
  - Enable Flicker: true
  - Flicker Speed: 5
  - Flicker Amount: 0.1
```

---

## 7️⃣ ВРАГИ (ОПЦИОНАЛЬНО)

### Enemy с EnemyMove

**Создание:**
```
1. Hierarchy → ПКМ → 2D Object → Sprite
2. Назвать: "Enemy"
3. Sprite Renderer → Sprite: спрайт врага
4. Add Component → Rigidbody2D
5. Add Component → Collider2D
6. Add Component → EnemyMove
```

**Настройка:**
```
EnemyMove (Script):
  - Настроить по необходимости
```

---

## ✅ ФИНАЛЬНАЯ СТРУКТУРА HIERARCHY

```
Scene:
  ├─ Main Camera
  │    ├─ Camera
  │    ├─ CameraFollow (Script)
  │    │    - Target: Player
  │    └─ CameraSkyboxFix (Script)
  │
  ├─ Canvas
  │    ├─ ScoreText (TMP)
  │    ├─ OrderText (TMP)
  │    └─ CheckOrderButton
  │         └─ Text (TMP)
  │
  ├─ EventSystem
  │
  ├─ ScoreManager
  │    └─ ScoreManager (Script)
  │         - Score Text: ScoreText
  │
  ├─ OrderSystem
  │    └─ OrderSystem (Script)
  │         - Min: 2, Max: 5
  │
  ├─ Player
  │    ├─ Sprite Renderer
  │    ├─ Rigidbody2D
  │    ├─ Box Collider 2D
  │    ├─ PlayerMove (Script)
  │    │    - Rgb: Rigidbody2D
  │    ├─ PlayerCatch (Script)
  │    │    - Hand Transform: Hand
  │    ├─ Animator
  │    └─ Hand (Empty)
  │
  ├─ ConveyorSpawner
  │    └─ ConveyorSpawner (Script)
  │         - Korzh Sprites: [массив]
  │         - Platform Sprite: спрайт
  │
  ├─ VerticalConveyor
  │    ├─ Sprite Renderer
  │    ├─ Box Collider 2D (Trigger)
  │    └─ VerticalConveyor (Script)
  │
  ├─ PC
  │    ├─ Sprite Renderer
  │    └─ OrderDisplay (Script)
  │         - Order Text: OrderText
  │
  ├─ UnderCake
  │    ├─ Sprite Renderer
  │    ├─ Circle Collider 2D (Trigger)
  │    └─ UnderCakePlate (Script)
  │
  └─ Prefabs (в папке Assets/Prefabs):
       └─ Korzh
            ├─ Sprite Renderer
            ├─ Rigidbody2D
            ├─ Box Collider 2D
            └─ PickupObject (Script)
```

---

## 📋 ЧЕК-ЛИСТ ВСЕХ ОБЪЕКТОВ

### Система заказов:
- [ ] ScoreManager создан
- [ ] OrderSystem создан
- [ ] PC имеет OrderDisplay
- [ ] UnderCake имеет UnderCakePlate
- [ ] UnderCake имеет Circle Collider 2D (Trigger)

### UI:
- [ ] Canvas существует
- [ ] ScoreText создан и привязан
- [ ] OrderText создан и привязан
- [ ] CheckOrderButton создан и привязан

### Игрок:
- [ ] Player имеет Rigidbody2D
- [ ] Player имеет Collider2D
- [ ] Player имеет PlayerMove
- [ ] Player имеет PlayerCatch
- [ ] Hand создан как дочерний объект

### Конвейер:
- [ ] ConveyorSpawner создан
- [ ] VerticalConveyor создан
- [ ] Korzh Prefab создан
- [ ] Спрайты коржей назначены

### Камера:
- [ ] Main Camera имеет CameraFollow
- [ ] Target камеры = Player

### Связи:
- [ ] ScoreManager → ScoreText
- [ ] PC → OrderText
- [ ] Button → UnderCake.CheckOrderButton
- [ ] CameraFollow → Player
- [ ] PlayerCatch → Hand

---

## 🎮 ТЕСТИРОВАНИЕ

### 1. Запустить игру (Play)

**Должно произойти:**
```
✓ Появился заказ: "ЗАКАЗ: X коржей"
✓ Очки: "Очки: 0"
✓ Кнопка "Проверить заказ" видна
✓ Камера следует за игроком
✓ Игрок может двигаться (WASD)
✓ Коржи спавнятся на конвейере
✓ Конвейер движется
```

### 2. Проверить подбор

**Действия:**
```
1. Подойти к коржу
2. Нажать E
3. Корж должен подобраться
4. Нажать E снова
5. Корж должен выброситься
```

### 3. Проверить заказ

**Действия:**
```
1. Подобрать нужное количество коржей
2. Сложить на UnderCake
3. Нажать кнопку "Проверить заказ"
4. Проверить Console:
   - "✅ ЗАКАЗ ПРАВИЛЬНЫЙ!" - если правильно
   - "❌ ЗАКАЗ НЕПРАВИЛЬНЫЙ!" - если неправильно
5. Коржи должны исчезнуть
6. Должен появиться новый заказ
```

---

**Следуйте инструкции шаг за шагом для настройки всех объектов!** 🎯
