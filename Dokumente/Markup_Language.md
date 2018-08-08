# Markup-Elemente
## Überschriften
Überschriften werden mit dem '#'-Symbol eingeleitet. Die Anzahl der '#'-Symbole gibt die Tiefe der Überschrift an. Es werden 5 Levels von Überschriften unterstützt. Eine Überschrift kann nur in einer neuen Zeile beginnen und muss mit einem Zeilenumbruch enden.

``` markdown
# Level1
## Level2
### Level3
#### Level4
##### Level5
```

## Textblock
Ein Element welches mit einem Text beginnt und diesen darstellt.
Ein Newline-Zeichen (\n, \r) wird ignoriert, wenn das nachfolgende Zeichen nicht ein anderes Markup-Element einführt.. Zwei oder mehr beenden den Textblock.

### Inline-Styles
#### Fett
#### Kursiv
#### Links
#### Unterstrichen
#### Durchgestrichen
#### Farbiger Text
#### Italic
#### Superscript
#### Subscript
#### Monospaced
#### Footnote
#### Mathe-Formeln
Mathe-Formulen müssen in jeweils zwei $-Zeichen eingeschlossen sein.

``` markdown
Eine lineare Funktion: $$f(x)=a*mx$$
```

## Listen und Aufzählungen
### Listen
Listen beginnen mit einem Dash (-). Man kann verschachtelte Listen erstellen, durch mehrmaliges Einrücken mit Dash.
Ein newline-Charakter wird ignoriert. 2 oder mehr beenden den Listenblock.

``` markdown
- Hallo
- Hallo
- Hallo
-- Huhu
-- Huhu
- Hihi
```


### Aufzählungen
Aufzählungen beginnen mit einem Asterix (*). Man kann verschachtelte Aufzählungen erstellen, durch mehrmaliges Einrücken mit Asterix.
Ein newline-Charakter wird ignoriert. 2 oder mehr beenden den Listenblock.

``` markdown
* Hallo
* Hallo
* Hallo
** Huhu
** Huhu
* Hihi
```


Grundsätzlich können Listen und Aufzählungen ineinander verschachtelt werden.


## Codeblock

## Quotation

## Horizontal Rule

## Bilder

## Videos

## Tabelle

## Matheblock

# Future-Blocks
## Charts
### Flowchart
- http://flowchart.js.org/
- https://github.com/BoostIO/Boostnote/wiki/Diagram-support

### PlantUML

- https://github.com/BoostIO/Boostnote/wiki/Diagram-support
- https://github.com/BoostIO/Boostnote/wiki/Diagram-support

### Sequenzdiagramm

- https://bramp.github.io/js-sequence-diagrams/
- https://github.com/BoostIO/Boostnote/wiki/Diagram-support

## Interactive Code
Python-Code Ausführung.Zum Plotten von Grafiken