# Lottery Archive

## Главное окно (MainWindow)

### Кнопка "Добавить участника"

* Всегда активна.
* Открывает окно AddParticipantWindow для ввода ФИО, баланса и жадности.
* После ввода и подтверждения (кнопка "ОК") создаётся новый участник и добавляется в таблицу "Участники".

### Кнопка "Редактировать участника"

* Активна только при выделенном участнике в таблице "Участники".
* Открывает окно AddParticipantWindow с предзаполненными полями выбранного участника.
* После внесения изменений и нажатия "ОК" обновляет информацию об участнике в списке.

### Кнопка "Создать лотерею"

* Активна, если в таблице "Участники" присутствует хотя бы один участник.
* Открывает окно CreateLotteryWindow, в котором можно ввести название лотереи, общее количество билетов, призовой фонд и выбрать участников.
* После подтверждения создаётся объект лотереи, номера билетов распределяются случайным образом между выбранными участниками, и лотерея сохраняется в файл (JSON или XML).
* В основной таблице "Лотереи" появляется строка с только что созданной лотереей.

### Кнопка "Загрузить статистику"

* Всегда активна.
* При нажатии пытается загрузить файл saved\_lottery.json или saved\_lottery.xml из каталога приложения.
* Если файл найден и успешно десериализован, обновляет таблицы "Лотереи" и "Участники" в соответствии с загруженными данными.
* Если файл отсутствует или произошла ошибка, в области "Статус" выводится соответствующее сообщение.

### Кнопка "Показать графики"

* Активна, если в таблице "Участники" установлен хотя бы один флажок в колонке "График".
* Открывает окно GraphsWindow, в котором рисуются два столбчатых графика: "Потрачено" и "Выиграно" по отмеченным участникам.
* Если данных по отмеченным участникам недостаточно, выводится сообщение об отсутствии данных для соответствующего графика.

### Выпадающий список "Формат"

* Содержит два варианта: JSON (по умолчанию) и XML.
* При выборе нового формата автоматически конвертирует все файлы лотерей в каталоге приложения из старого формата в новый.
* После завершения миграции в области "Статус" отображается сообщение о результате.

### Таблица "Лотереи" (DataGrid)

* Отображает список текущих лотерей.
* Столбцы:

  * Название
  * Всего билетов
  * Призовой фонд
  * Продано
* Изменить содержимое данной таблицы напрямую нельзя — она заполняется при создании или загрузке лотереи.

### Таблица "Участники" (DataGrid)

* Отображает список всех участников.
* Столбцы:

  * График (чекбокс, отмечает участника для вывода графиков)
  * ФИО
  * Баланс
  * Жадность
  * Билетов (количество купленных билетов)
* Выбор строки в этой таблице активирует кнопку "Редактировать участника".
* Установка/снятие флажка в колонке "График" влияет на доступность кнопки "Показать графики".

### Область "Статус"

* Находится в правой колонке основного окна.
* Показывает текущее сообщение о состоянии приложения:

  * Успешное создание или загрузка лотереи.
  * Ошибки при валидации или сохранении.
  * Результат миграции форматов.
  * Информацию о сохранении статистики.

---

## Диалоговое окно добавления/редактирования участника (AddParticipantWindow)

### Поля ввода

* Имя (обязательно)
* Фамилия (обязательно)
* Отчество (необязательно)
* Баланс (неотрицательное число, при редактировании корректно изменяется через методы AddMoney/TrySpendMoney)
* Жадность (число от 0 до 1)

### Кнопка "ОК"

* Валидирует, что имя и фамилия не пустые, баланс ≥ 0, жадность в диапазоне \[0,1].
* Если окно открылось для создания участника, создаёт новый объект LotteryParticipant и добавляет в коллекцию в MainWindow.
* Если окно открылось для редактирования, обновляет у существующего участника ФИО, баланс и жадность.
* После успешного выполнения закрывает окно.

### Кнопка "Отмена"

* Закрывает окно без внесения изменений.

---

## Диалоговое окно создания лотереи (CreateLotteryWindow)

### Поля ввода

* Название (обязательно, не может быть пустым)
* Всего билетов (целое положительное число, обязательно)
* Призовой фонд (десятичное число ≥ 0, обязательно)
* Список участников (ListBox с множественным выбором, DisplayMemberPath="FullName")

### Кнопка "ОК"

* Валидирует, что название заполнено, количество билетов > 0, призовой фонд ≥ 0 и выбран хотя бы один участник.
* Создаёт объект Lottery, где каждому выбранному участнику пытается продать по билетам в случайном порядке до исчерпания общего количества.
* Сохраняет созданную лотерею в файл saved\_lottery.json или saved\_lottery.xml в каталоге приложения.
* Закрывает окно.

### Кнопка "Отмена"

* Закрывает окно без создания лотереи.

---

## Окно графиков (GraphsWindow)

### Заголовок

* "Графики: потрачено vs выиграно"

### График "Потрачено"

* Столбчатая диаграмма по отмеченным участникам.
* По оси X — ФИО участника.
* По оси Y — сумма, потраченная участником (TotalSpent).
* Если отмеченные участники отсутствуют или у них нет данных, отображается сообщение: "Нет данных для графика «Потрачено»."

### График "Выиграно"

* Столбчатая диаграмма по тем же участникам.
* По оси X — ФИО участника.
* По оси Y — сумма, выигранная участником (TotalWon).
* Если данных нет, отображается сообщение: "Нет данных для графика «Выиграно»."

### Закрытие окна

* Окно открывается модально из MainWindow по кнопке "Показать графики".
* Закрывается стандартной кнопкой в заголовке окна.

---

## Окно статистики (StatisticsWindow) (опционально)

### Заголовок

* "Статистика лотереи"

### Кнопка "Сохранить статистику"

* Собирает данные: общее количество проданных билетов, призовой фонд и список пар (Имя участника, TicketsCount).
* Сохраняет их в файл stats\_YYYYMMDD\_HHMMSS.json в каталоге приложения.
* После успешного сохранения выводится сообщение о созданном файле.
* В случае ошибки выводит сообщение об ошибке.

### Кнопка "Закрыть"

* Закрывает окно без сохранения.

### График

* Столбчатая диаграмма по количеству купленных билетов:

  * По оси X — ФИО участников.
  * По оси Y — количество билетов (TicketsCount).
* Если данных нет, отображается текст "Нет данных для отображения графика."

---

## Converter: NullToBoolConverter

* Предназначен для привязки свойства IsEnabled к наличию выбранного участника.
* Если значение null, возвращает False; если не null, возвращает True.
* Используется в MainWindow для кнопки "Редактировать участника":
  IsEnabled="{Binding SelectedParticipant, Converter={StaticResource NullToBoolConverter}}"

---

## Как открыть и закрыть окна

1. **AddParticipantWindow**

   * Открывается из MainWindow: кнопки "Добавить участника" или "Редактировать участника".
   * Закрывается кнопками "ОК" или "Отмена".

2. **CreateLotteryWindow**

   * Открывается из MainWindow: кнопка "Создать лотерею" (если есть хотя бы один участник).
   * Закрывается кнопками "ОК" или "Отмена".

3. **GraphsWindow**

   * Открывается из MainWindow: кнопка "Показать графики" (если хотя бы один участник отмечен для графика).
   * Закрывается стандартной кнопкой в заголовке окна.

4. **StatisticsWindow**

   * (Если реализовано) открывается по соответствующей команде.
   * Закрывается кнопкой "Закрыть" или стандартным закрытием окна.

---

## Краткое описание взаимодействия

* В MainWindow пользователь может добавлять и редактировать участников, затем создавать лотерею или загружать предыдущую статистику.
* В списке "Участники" наличие флажков "График" влияет на активацию кнопки "Показать графики".
* Переключение формата JSON/XML в верхней части MainWindow автоматически конвертирует файлы лотерей.
* Все окна создаются модально и возвращают результат через DialogResult (true/false) для подтверждения или отмены действий.
