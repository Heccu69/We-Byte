# ПОЛНАЯ НАСТРОЙКА ВСЕХ ОБЪЕКТОВ В UNITY

## 🎯 Что нужно создать

1. **ScoreManager** - управление очками
2. **OrderSystem** - управление заказами
3. **Canvas** - UI интерфейс
4. **ScoreText** - отображение очков
5. **OrderText** - отображение заказа на PC
6. **CheckOrderButton** - кнопка проверки заказа
7. **PC** - объект с OrderDisplay
8. **UnderCake** - тарелка с UnderCakePlate

---

## 📋 ПОШАГОВАЯ ИНСТРУКЦИЯ

### ШАГ 1: Создать ScoreManager

```
1. Hierarchy → ПКМ → Create Empty
2. Назвать: "ScoreManager"
3. Inspector → Add Component → ScoreManager
4. Оставить пустым (ссылку на ScoreText добавим позже)
```

**Результат:**
```
Hierarchy:
  └─ ScoreManager
       └─ ScoreManager (Script)
            - Score Text: None (пока)
```

---

### ШАГ 2: Создать OrderSystem

```
1. Hierarchy → ПКМ → Create Empty
2. Назвать: "OrderSystem"
3. Inspector → Add Component → OrderSystem
4. Настроить:
   - Min Korzh Count: 2
   - Max Korzh Count: 5
```

**Результат:**
```
Hierarchy:
  └─ OrderSystem
       └─ OrderSystem (Script)
            - Min Korzh Count: 2
            - Max Korzh Count: 5
```

---

### ШАГ 3: Создать Canvas (если нет)

```
1. Hierarchy → ПКМ → UI → Canvas
2. Canvas уже должен быть в сцене
3. Если нет - создать новый
4. Настроить Canvas:
   - Render Mode: Screen Space - Overlay
   - Canvas Scaler → UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080
```

**Результат:**
```
Hierarchy:
  └─ Canvas
       ├─ Canvas (Script)
       ├─ Canvas Scaler (Script)
       └─ Graphic Raycaster (Script)
```

---

### ШАГ 4: Создать ScoreText (очки)

```
1. Canvas → ПКМ → UI → Text - TextMeshPro
2. Назвать: "ScoreText"
3. Настроить:
   - Text: "Очки: 0"
   - Font Size: 36
   - Alignment: Center
   - Color: белый
4. Rect Transform:
   - Anchor Presets: Top Center
   - Pos X: 0
   - Pos Y: -50
   - Width: 300
   - Height: 60
```

**Результат:**
```
Hierarchy:
  └─ Canvas
       └─ ScoreText (TextMeshPro)
            - Text: "Очки: 0"
            - Font Size: 36
```

**ВАЖНО:** Теперь привязать к ScoreManager:
```
1. Выбрать ScoreManager в Hierarchy
2. Inspector → ScoreManager (Script)
3. Перетащить ScoreText в поле "Score Text"
```

---

### ШАГ 5: Создать OrderText (заказ)

```
1. Canvas → ПКМ → UI → Text - TextMeshPro
2. Назвать: "OrderText"
3. Настроить:
   - Text: "ЗАКАЗ:\n3 коржей"
   - Font Size: 32
   - Alignment: Center
   - Color: желтый или оранжевый
4. Rect Transform:
   - Anchor Presets: Top Left
   - Pos X: 150
   - Pos Y: -100
   - Width: 250
   - Height: 100
```

**Результат:**
```
Hierarchy:
  └─ Canvas
       └─ OrderText (TextMeshPro)
            - Text: "ЗАКАЗ:\n3 коржей"
            - Font Size: 32
```

---

### ШАГ 6: Создать CheckOrderButton (кнопка)

```
1. Canvas → ПКМ → UI → Button - TextMeshPro
2. Назвать: "CheckOrderButton"
3. Настроить кнопку:
   - Rect Transform:
     - Anchor Presets: Bottom Center
     - Pos X: 0
     - Pos Y: 100
     - Width: 300
     - Height: 80
4. Настроить текст кнопки:
   - Раскрыть CheckOrderButton
   - Выбрать Text (TMP)
   - Text: "Проверить заказ"
   - Font Size: 28
   - Color: белый
5. Настроить цвета кнопки:
   - Normal Color: зеленый (0, 200, 0)
   - Highlighted Color: светло-зеленый (100, 255, 100)
   - Pressed Color: темно-зеленый (0, 150, 0)
```

**Результат:**
```
Hierarchy:
  └─ Canvas
       └─ CheckOrderButton (Button)
            ├─ Button (Script)
            └─ Text (TMP)
                 - Text: "Проверить заказ"
```

**ВАЖНО:** Привязку к UnderCake сделаем позже!

---

### ШАГ 7: Настроить PC (объект с заказом)

**ВАРИАНТ A: Если PC уже есть в сцене**

```
1. Найти объект "PC" в Hierarchy
2. Inspector → Add Component → OrderDisplay
3. Перетащить OrderText в поле "Order Text"
```

**ВАРИАНТ B: Если PC нет**

```
1. Hierarchy → ПКМ → Create Empty
2. Назвать: "PC"
3. Inspector → Add Component → Sprite Renderer (опционально)
4. Inspector → Add Component → OrderDisplay
5. Перетащить OrderText в поле "Order Text"
6. Настроить позицию PC в сцене
```

**Результат:**
```
Hierarchy:
  └─ PC
       └─ OrderDisplay (Script)
            - Order Text: OrderText
            - Order Panel: None (опционально)
```

---

### ШАГ 8: Настроить UnderCake (тарелка)

**ВАРИАНТ A: Если UnderCake уже есть в сцене**

```
1. Найти объект "UnderCake" в Hierarchy
2. Inspector → Add Component → UnderCakePlate
3. Настроить:
   - Check Radius: 2
   - Stack Tolerance: 0.3
   - Show Debug Info: true (для отладки)
4. Добавить коллайдер (если нет):
   - Add Component → Circle Collider 2D
   - Is Trigger: true
   - Radius: 1.5 (подобрать по размеру тарелки)
```

**ВАРИАНТ B: Если UnderCake нет**

```
1. Hierarchy → ПКМ → 2D Object → Sprite
2. Назвать: "UnderCake"
3. Inspector → Sprite Renderer:
   - Sprite: выбрать спрайт тарелки
4. Add Component → Circle Collider 2D
   - Is Trigger: true
   - Radius: 1.5
5. Add Component → UnderCakePlate
   - Check Radius: 2
   - Stack Tolerance: 0.3
   - Show Debug Info: true
6. Настроить позицию в сцене (где игрок будет складывать коржи)
```

**Результат:**
```
Hierarchy:
  └─ UnderCake
       ├─ Sprite Renderer
       ├─ Circle Collider 2D
       │    - Is Trigger: true
       │    - Radius: 1.5
       └─ UnderCakePlate (Script)
            - Check Radius: 2
            - Stack Tolerance: 0.3
            - Show Debug Info: true
```

---

### ШАГ 9: Привязать кнопку к UnderCake

```
1. Выбрать CheckOrderButton в Hierarchy
2. Inspector → Button (Script)
3. Найти раздел "On Click ()"
4. Нажать "+" (добавить событие)
5. Перетащить объект UnderCake в поле "None (Object)"
6. В выпадающем меню выбрать:
   UnderCakePlate → CheckOrderButton ()
7. Убедиться что выбрано "Runtime Only"
```

**Результат:**
```
CheckOrderButton:
  └─ Button (Script)
       └─ On Click ()
            - Runtime Only
            - UnderCake
            - UnderCakePlate.CheckOrderButton
```

---

## ✅ ФИНАЛЬНАЯ ПРОВЕРКА

### Hierarchy должна выглядеть так:

```
Scene:
  ├─ ScoreManager
  │    └─ ScoreManager (Script)
  │         - Score Text: ScoreText ✓
  │
  ├─ OrderSystem
  │    └─ OrderSystem (Script)
  │         - Min Korzh Count: 2
  │         - Max Korzh Count: 5
  │
  ├─ Canvas
  │    ├─ ScoreText (TMP)
  │    ├─ OrderText (TMP)
  │    └─ CheckOrderButton
  │         └─ Button (Script)
  │              - On Click: UnderCake.CheckOrderButton ✓
  │
  ├─ PC
  │    └─ OrderDisplay (Script)
  │         - Order Text: OrderText ✓
  │
  └─ UnderCake
       └─ UnderCakePlate (Script)
            - Check Radius: 2
            - Stack Tolerance: 0.3
```

---

## 🧪 ТЕСТИРОВАНИЕ

### 1. Проверить что все ссылки заполнены:

```
✓ ScoreManager → Score Text: ScoreText
✓ OrderDisplay → Order Text: OrderText
✓ CheckOrderButton → On Click: UnderCake.CheckOrderButton
```

### 2. Запустить игру (Play):

```
1. Должен появиться заказ на OrderText: "ЗАКАЗ: X коржей"
2. ScoreText должен показывать: "Очки: 0"
3. Кнопка "Проверить заказ" должна быть видна
```

### 3. Проверить Console:

```
При запуске игры должно быть:
"Новый заказ: 3 коржей" (или другое число)
```

### 4. Проверить кнопку:

```
1. Положить коржи на тарелку UnderCake
2. Нажать кнопку "Проверить заказ"
3. В Console должно появиться:
   "Проверка заказа: коржей в стопке = X"
   "✅ ЗАКАЗ ПРАВИЛЬНЫЙ!" или "❌ ЗАКАЗ НЕПРАВИЛЬНЫЙ!"
```

---

## 🐛 ЕСЛИ ЧТО-ТО НЕ РАБОТАЕТ

### Проблема 1: "Заказ не отображается"

**Проверить:**
```
1. PC → OrderDisplay → Order Text заполнено?
2. OrderText существует в Canvas?
3. OrderSystem существует в сцене?
```

### Проблема 2: "Очки не отображаются"

**Проверить:**
```
1. ScoreManager → Score Text заполнено?
2. ScoreText существует в Canvas?
```

### Проблема 3: "Кнопка не работает"

**Проверить:**
```
1. CheckOrderButton → On Click заполнено?
2. UnderCake перетащен в поле объекта?
3. Выбран метод CheckOrderButton()?
4. UnderCakePlate прикреплен к UnderCake?
```

### Проблема 4: "Коржи не считаются"

**Проверить:**
```
1. UnderCake → UnderCakePlate существует?
2. UnderCake → Circle Collider 2D существует?
3. Circle Collider 2D → Is Trigger: true?
4. Check Radius достаточно большой (2)?
```

---

## 📝 КРАТКИЙ ЧЕК-ЛИСТ

- [ ] ScoreManager создан
- [ ] OrderSystem создан
- [ ] Canvas существует
- [ ] ScoreText создан и привязан к ScoreManager
- [ ] OrderText создан и привязан к PC
- [ ] CheckOrderButton создан
- [ ] PC имеет OrderDisplay
- [ ] UnderCake имеет UnderCakePlate
- [ ] UnderCake имеет Circle Collider 2D (Is Trigger: true)
- [ ] Кнопка привязана к UnderCake.CheckOrderButton
- [ ] Все ссылки заполнены (нет "None")
- [ ] При запуске игры появляется заказ
- [ ] При нажатии кнопки появляются логи

---

## 🎯 ВАЖНЫЕ ПОЗИЦИИ В СЦЕНЕ

### UnderCake (тарелка):
```
Position: где-то на уровне пола
Например: (0, -2, 0)
Должна быть доступна игроку
```

### PC (компьютер):
```
Position: где-то на столе
Например: (5, 0, 0)
OrderText будет отображаться в UI (не в мире)
```

### Canvas:
```
Render Mode: Screen Space - Overlay
Всегда поверх всего
```

---

**Следуйте инструкции шаг за шагом, и все заработает!** 🎉
