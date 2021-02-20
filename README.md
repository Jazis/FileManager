# FileManager

Простой файловый менеджер.
Позволяет просматривать директории и файлы. при этом отображение идет 2ух видов в treeView и webBrowser.

<h3>Натройки</h3>
При нажатии кнопки "Search" в главном окне - открывается окно для поиска необходимых файлов и директорий. Начальный путь и название файла, пользователь может указать самостоятельно.
Все пути и запрос сохраняются в файле "cfg.ini", в папке с программой. Если файла для сохранения настроек нету, то программа сма создаст его и запишет все заново.<br>
Поле для поиска(Request) принимает как и регулярные выражения, так и простые запросы.<br>
Также в окне поиска можно найти несколько счетчиков:<br>
- Folders checked - Количество просмотренных папок<br>
- Files checked - Количество просмотренных файлов<br>
- result - Сколько удалось найти подходящих вариантов<br>
- Time(sec) - Сукундомер, оторбражает количество секунд с начала старта поиска<br>

<h4>Формат строк файла "cfg.ini"</h4>
Формат строки конфигурации - [определение принадлежности]=строка<br>
[LSEARCH] - запрос<br>
[PATHS] - пути для поиска<br>


<h2>Сериншоты</h2>
![Скриншот основного окна](https://i.imgur.com/S0rpSrh.png)

![Скриншот окна настроек](https://i.imgur.com/wUqgAGD.png)

![Image alt](https://i.imgur.com/mjvYqSj.gif)
