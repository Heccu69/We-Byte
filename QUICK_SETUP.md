# БЫСТРАЯ НАСТРОЙКА - ШПАРГАЛКА

## 📋 8 ОБЪЕКТОВ ДЛЯ СОЗДАНИЯ

### 1️⃣ ScoreManager
```
Create Empty → "ScoreManager"
Add Component → ScoreManager
```

### 2️⃣ OrderSystem
```
Create Empty → "OrderSystem"
Add Component → OrderSystem
Min: 2, Max: 5
```

### 3️⃣ Canvas
```
UI → Canvas (если нет)
```

### 4️⃣ ScoreText
```
Canvas → UI → Text - TextMeshPro → "ScoreText"
Text: "Очки: 0"
Top Center, Font: 36
```

### 5️⃣ OrderText
```
Canvas → UI → Text - TextMeshPro → "OrderText"
Text: "ЗАКАЗ:\n3 коржей"
Top Left, Font: 32
```

### 6️⃣ CheckOrderButton
```
Canvas → UI → Button - TextMeshPro → "CheckOrderButton"
Text: "Проверить заказ"
Bottom Center, 300x80
```

### 7️⃣ PC (если нет)
```
Найти PC в сцене
Add Component → OrderDisplay
```

### 8️⃣ UnderCake (если нет)
```
Найти UnderCake в сцене
Add Component → Circle Collider 2D (Is Trigger: true)
Add Component → UnderCakePlate
```

---

## 🔗 СВЯЗАТЬ ОБЪЕКТЫ

### ScoreManager ← ScoreText
```
ScoreManager → Score Text ← перетащить ScoreText
```

### PC ← OrderText
```
PC → OrderDisplay → Order Text ← перетащить OrderText
```

### CheckOrderButton ← UnderCake
```
CheckOrderButton → On Click → "+"
Перетащить UnderCake
UnderCakePlate → CheckOrderButton()
```

---

## ✅ ПРОВЕРКА

```
✓ ScoreManager → Score Text: ScoreText
✓ PC → Order Text: OrderText
✓ Button → On Click: UnderCake.CheckOrderButton
✓ UnderCake → Circle Collider 2D (Is Trigger: true)
✓ UnderCake → UnderCakePlate (Check Radius: 2)
```

---

## 🎮 ТЕСТ

```
Play → Должен появиться заказ
Положить коржи → Нажать кнопку
Console: "✅ ЗАКАЗ ПРАВИЛЬНЫЙ!" или "❌ ЗАКАЗ НЕПРАВИЛЬНЫЙ!"
```

---

**Полная инструкция:** `UNITY_FULL_SETUP.md`
