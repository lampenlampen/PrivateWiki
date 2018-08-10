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
- - Huhu
- - Huhu
- Hihi
```


### Aufzählungen
Aufzählungen beginnen mit einem Asterix (*). Man kann verschachtelte Aufzählungen erstellen, durch mehrmaliges Einrücken mit Asterix.
Ein newline-Charakter wird ignoriert. 2 oder mehr beenden den Listenblock.

``` markdown
* Hallo
* Hallo
* Hallo
* * Huhu
* * Huhu
* Hihi
*  * dsfsdasd
*  * asdfasd
```

### Checkboxen
Checkboxen werden mit einer eckigen offenen Klammer und einer folgenden eckigen geschlossenen Klammer eingeleitet.
``` markdown
[] Hallo
```
Um eine Checkbox abzuhacken kann zwischen die Klammer ein kleines oder großes x geschrieben werden.

``` markdown
[x] Hallo
[X] Huhu
```


Grundsätzlich können Listen und Aufzählungen ineinander verschachtelt werden.


## Codeblock

A Codeblock start with 3 dashes (```).
After the 3 dashes a language can be specified.

Supported Languages:

Beispiel:
``` markdown
    ```
    public class Codeblock
    {
        public String code;
    }
    ```
```

## Quotation

## Horizontal Line
---

## Bilder

## Videos

## Tabelle

A Tableblock starts with a vertical bar ( | ).
All rows have to start with a vertical bar.
Columns are separated with a vertical bar.

After the first line follows a special line.
The contents of each cell are three dashes (---).
This indicates the line above is a header line.

Colons on the left, the right or on both sides can be used to align columns left, right or middle.
Default Alignment is left.

``` markdown
| Header 1 | Header 2 | Header3 | Header4 |
|:---|---:|:---:|:---|
|left | right | middle | left |
| sadfhk | aspdfkhj | pakdfh | padfhj |
|sadf|sdf|sdf|sdf|
```
> | Header 1 | Header 2 | Header3 | Header4 |
> |:---|---:|:---:|:---|
> |left | right | middle | left |
> | sadfgdfgdfhk | sdfg | pakdfh | padsdfsadfgfhj |
> |sadf|sdf|sdasdfgasdgf|sdf|


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
Python-Code Ausführung. Zum Plotten von Grafiken


Hallo

> huhuhasdfökajsdfcsdöklcnsaödfjsaöfdhsdfjkglhdfj
> sdfgklhauewilfnsv
> fsdfkljghnalkf
> > sdafjkalhfasjklhf
> > 
> > sdfölgkjearv
> > sdvkhsdfvg
> > sdlkfndfkdnhfgkdlsjg
> > > hsdfkjh
> > > sdkjfhasdf
> > > sdflkghjdsfjkg  sfgadgafdgadfgadfgs
> > > sdfglkj
> 
> 
> dfslgknsdflvknfdadkjdsfh
> sdfvkjnbasdfsdf
> asdklfjh
> 1. sdfjkhsjkdfh
> 2. sdkljfhds
> 3. sdfkjhsdf
> [Hallo](https://www.wikipedia.com)