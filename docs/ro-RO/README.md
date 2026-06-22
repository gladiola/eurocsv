# EuroCSV — Convertor de localizare CSV

EuroCSV ajută echipele să convertească fișiere CSV între formate internaționale folosite în afaceri. Aplicația ajustează separatoarele de câmp, separatorii zecimali, separatorii de mii și formatele de dată, astfel încât datele să fie importate corect în regiunea de destinație.

> **Notă:** ro-RO folosește de obicei `;` ca separator de câmpuri, `,` ca separator zecimal, `.` ca separator de mii și `dd.MM.yyyy` ca format de dată.

---

## Convenții CSV după setări regionale

| Setări regionale | Separator | Zecimal | Mii | Format dată |
|---|---|---|---|---|
| en-US — English (United States) | , | . | , | MM/dd/yyyy |
| en-GB — English (United Kingdom) | , | . | , | dd/MM/yyyy |
| de-DE — German (Germany) | ; | , | . | dd.MM.yyyy |
| de-AT — German (Austria) | ; | , | . | dd.MM.yyyy |
| de-CH — German (Switzerland) | ; | . | ' | dd.MM.yyyy |
| fr-FR — French (France) | ; | , | space | dd/MM/yyyy |
| fr-BE — French (Belgium) | ; | , | . | dd/MM/yyyy |
| fr-CH — French (Switzerland) | ; | . | ' | dd.MM.yyyy |
| es-ES — Spanish (Spain) | ; | , | . | dd/MM/yyyy |
| es-MX — Spanish (Mexico) | , | . | , | dd/MM/yyyy |
| it-IT — Italian (Italy) | ; | , | . | dd/MM/yyyy |
| nl-NL — Dutch (Netherlands) | ; | , | . | dd-MM-yyyy |
| nl-BE — Dutch (Belgium) | ; | , | . | dd/MM/yyyy |
| pt-PT — Portuguese (Portugal) | ; | , | . | dd/MM/yyyy |
| pt-BR — Portuguese (Brazil) | ; | , | . | dd/MM/yyyy |
| pl-PL — Polish (Poland) | ; | , | space | dd.MM.yyyy |
| cs-CZ — Czech (Czech Republic) | ; | , | space | dd.MM.yyyy |
| sk-SK — Slovak (Slovakia) | ; | , | space | dd.MM.yyyy |
| ru-RU — Russian (Russia) | ; | , | space | dd.MM.yyyy |
| sv-SE — Swedish (Sweden) | ; | , | space | yyyy-MM-dd |
| da-DK — Danish (Denmark) | ; | , | . | dd-MM-yyyy |
| nb-NO — Norwegian (Norway) | ; | , | space | dd.MM.yyyy |
| fi-FI — Finnish (Finland) | ; | , | space | dd.MM.yyyy |
| hu-HU — Hungarian (Hungary) | ; | , | space | yyyy.MM.dd |
| ro-RO — Romanian (Romania) | ; | , | . | dd.MM.yyyy |
| tr-TR — Turkish (Turkey) | ; | , | . | dd.MM.yyyy |
| ja-JP — Japanese (Japan) | , | . | , | yyyy/MM/dd |
| zh-CN — Chinese Simplified (China) | , | . | , | yyyy/MM/dd |
| el-GR — Greek (Greece) | ; | , | . | dd/MM/yyyy |
| bg-BG — Bulgarian (Bulgaria) | ; | , | space | dd.MM.yyyy |
| hr-HR — Croatian (Croatia) | ; | , | . | dd.MM.yyyy |
| uk-UA — Ukrainian (Ukraine) | ; | , | space | dd.MM.yyyy |
| lt-LT — Lithuanian (Lithuania) | ; | , | space | yyyy-MM-dd |
| lv-LV — Latvian (Latvia) | ; | , | space | dd.MM.yyyy |

---

## Utilizare

Încărcați fișierul CSV, selectați setările regionale sursă și destinație, apoi descărcați fișierul convertit.

---

→ [English documentation](../../README.md)
