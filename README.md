# eurocsv

Converts CSV files between international formatting conventions in seconds.

---

## 🌐 Language Index

| Language | Folder |
|---|---|
| English (United States) (`en-US`) | [docs/en-US/](docs/en-US/) |
| English (United Kingdom) (`en-GB`) | [docs/en-GB/](docs/en-GB/) |
| German (Germany) (`de-DE`) | [docs/de-DE/](docs/de-DE/) |
| German (Austria) (`de-AT`) | [docs/de-AT/](docs/de-AT/) |
| German (Switzerland) (`de-CH`) | [docs/de-CH/](docs/de-CH/) |
| French (France) (`fr-FR`) | [docs/fr-FR/](docs/fr-FR/) |
| French (Belgium) (`fr-BE`) | [docs/fr-BE/](docs/fr-BE/) |
| French (Switzerland) (`fr-CH`) | [docs/fr-CH/](docs/fr-CH/) |
| Spanish (Spain) (`es-ES`) | [docs/es-ES/](docs/es-ES/) |
| Spanish (Mexico) (`es-MX`) | [docs/es-MX/](docs/es-MX/) |
| Italian (Italy) (`it-IT`) | [docs/it-IT/](docs/it-IT/) |
| Dutch (Netherlands) (`nl-NL`) | [docs/nl-NL/](docs/nl-NL/) |
| Dutch (Belgium) (`nl-BE`) | [docs/nl-BE/](docs/nl-BE/) |
| Portuguese (Portugal) (`pt-PT`) | [docs/pt-PT/](docs/pt-PT/) |
| Portuguese (Brazil) (`pt-BR`) | [docs/pt-BR/](docs/pt-BR/) |
| Polish (Poland) (`pl-PL`) | [docs/pl-PL/](docs/pl-PL/) |
| Czech (Czech Republic) (`cs-CZ`) | [docs/cs-CZ/](docs/cs-CZ/) |
| Slovak (Slovakia) (`sk-SK`) | [docs/sk-SK/](docs/sk-SK/) |
| Russian (Russia) (`ru-RU`) | [docs/ru-RU/](docs/ru-RU/) |
| Swedish (Sweden) (`sv-SE`) | [docs/sv-SE/](docs/sv-SE/) |
| Danish (Denmark) (`da-DK`) | [docs/da-DK/](docs/da-DK/) |
| Norwegian (Norway) (`nb-NO`) | [docs/nb-NO/](docs/nb-NO/) |
| Finnish (Finland) (`fi-FI`) | [docs/fi-FI/](docs/fi-FI/) |
| Hungarian (Hungary) (`hu-HU`) | [docs/hu-HU/](docs/hu-HU/) |
| Romanian (Romania) (`ro-RO`) | [docs/ro-RO/](docs/ro-RO/) |
| Turkish (Turkey) (`tr-TR`) | [docs/tr-TR/](docs/tr-TR/) |
| Japanese (Japan) (`ja-JP`) | [docs/ja-JP/](docs/ja-JP/) |
| Chinese Simplified (China) (`zh-CN`) | [docs/zh-CN/](docs/zh-CN/) |
| Greek (Greece) (`el-GR`) | [docs/el-GR/](docs/el-GR/) |
| Bulgarian (Bulgaria) (`bg-BG`) | [docs/bg-BG/](docs/bg-BG/) |
| Croatian (Croatia) (`hr-HR`) | [docs/hr-HR/](docs/hr-HR/) |
| Ukrainian (Ukraine) (`uk-UA`) | [docs/uk-UA/](docs/uk-UA/) |
| Lithuanian (Lithuania) (`lt-LT`) | [docs/lt-LT/](docs/lt-LT/) |
| Latvian (Latvia) (`lv-LV`) | [docs/lv-LV/](docs/lv-LV/) |

---

## 📊 CSV Conventions by Locale

| Locale | Delimiter | Decimal | Thousands | Date Format |
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

## 🗂 Convention Pattern Summary

| Pattern | Convention | Locales |
|---|---|---|
| Field delimiter | `,` is used by English, Japanese, Simplified Chinese, and Mexican Spanish presets. | `en-US`, `en-GB`, `es-MX`, `ja-JP`, `zh-CN` |
| Field delimiter | `;` is used by the remaining continental European presets. | `de-*`, `fr-*`, `es-ES`, `it-IT`, `nl-*`, `pt-*`, `pl-PL`, `cs-CZ`, `sk-SK`, `ru-RU`, `sv-SE`, `da-DK`, `nb-NO`, `fi-FI`, `hu-HU`, `ro-RO`, `tr-TR`, `el-GR`, `bg-BG`, `hr-HR`, `uk-UA`, `lt-LT`, `lv-LV` |
| Decimal separator | `.` is used by English-speaking presets plus Swiss German, Swiss French, Mexican Spanish, Japanese, and Simplified Chinese. | `en-US`, `en-GB`, `de-CH`, `fr-CH`, `es-MX`, `ja-JP`, `zh-CN` |
| Decimal separator | `,` is used by most continental European presets. | All remaining 27 locales |
| Thousands separator | `,`, `.`, `space`, and `'` all appear in the built-in presets. | `,`: `en-US`, `en-GB`, `es-MX`, `ja-JP`, `zh-CN` · `.`: `de-DE`, `de-AT`, `fr-BE`, `es-ES`, `it-IT`, `nl-*`, `pt-*`, `da-DK`, `ro-RO`, `tr-TR`, `el-GR`, `hr-HR` · `space`: `fr-FR`, `pl-PL`, `cs-CZ`, `sk-SK`, `ru-RU`, `sv-SE`, `nb-NO`, `fi-FI`, `hu-HU`, `bg-BG`, `uk-UA`, `lt-LT`, `lv-LV` · `'`: `de-CH`, `fr-CH` |
| Date format | Month-first formatting is limited to the U.S. preset. | `en-US` → `MM/dd/yyyy` |
| Date format | Day-first slash formatting is common across the UK, Romance-language presets, and Greece. | `en-GB`, `fr-FR`, `fr-BE`, `es-ES`, `es-MX`, `it-IT`, `nl-BE`, `pt-PT`, `pt-BR`, `el-GR` |
| Date format | Dot-separated day-first formatting is common across Germanic, Central European, and Eastern European presets. | `de-DE`, `de-AT`, `de-CH`, `fr-CH`, `pl-PL`, `cs-CZ`, `sk-SK`, `ru-RU`, `nb-NO`, `fi-FI`, `ro-RO`, `tr-TR`, `bg-BG`, `hr-HR`, `uk-UA`, `lv-LV` |
| Date format | ISO-like year-first variants appear in Sweden, Lithuania, Hungary, Japan, and China. | `sv-SE`, `lt-LT`, `hu-HU`, `ja-JP`, `zh-CN` |
| Date format | Day-first dash formatting is specific to Dutch (Netherlands) and Danish presets. | `nl-NL`, `da-DK` |

## Features

- 34 built-in locale presets covering Europe plus widely used English and Asian CSV formats.
- Converts field delimiters, decimal separators, thousands separators, and date formats between source and target locales.
- Browser-based ASP.NET Core MVC interface for uploading, converting, and downloading files quickly.
- Optional thousands-separator conversion to avoid corrupting date-like values when separators overlap.

## Building and Running

```bash
dotnet restore
dotnet build eurocsv.slnx
dotnet run --project eurocsv/eurocsv.csproj
```

Then open the local URL printed by ASP.NET Core in your browser.

To run the automated tests:

```bash
dotnet test --nologo
```

## Deploying on Ubuntu with Nginx

This section covers a production deployment on Ubuntu where nginx terminates TLS and reverse-proxies to Kestrel.

### 1. Publish the application

```bash
dotnet publish eurocsv/eurocsv.csproj -c Release -o /var/www/eurocsv
```

### 2. Create a systemd service

Create `/etc/systemd/system/eurocsv.service`:

```ini
[Unit]
Description=EuroCSV ASP.NET Core application
After=network.target

[Service]
WorkingDirectory=/var/www/eurocsv
ExecStart=/usr/bin/dotnet /var/www/eurocsv/eurocsv.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5006
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

Enable and start the service:

```bash
sudo systemctl daemon-reload
sudo systemctl enable --now eurocsv
```

### 3. Configure nginx

Create `/etc/nginx/sites-available/eurocsv` and enable it with a symlink to `sites-enabled`:

```nginx
server {
    listen 80;
    server_name example.com;
    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl;
    server_name example.com;

    ssl_certificate     /etc/ssl/certs/example.com.crt;
    ssl_certificate_key /etc/ssl/private/example.com.key;

    location / {
        proxy_pass         http://localhost:5006;
        proxy_http_version 1.1;
        proxy_set_header   Host              $host;
        proxy_set_header   X-Real-IP         $remote_addr;
        proxy_set_header   X-Forwarded-For   $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_set_header   Upgrade           $http_upgrade;
        proxy_set_header   Connection        keep-alive;
        proxy_cache_bypass $http_upgrade;
    }
}
```

```bash
sudo ln -s /etc/nginx/sites-available/eurocsv /etc/nginx/sites-enabled/
sudo nginx -t && sudo systemctl reload nginx
```

> **Note:** The app calls `UseForwardedHeaders` before `UseHttpsRedirection` so it correctly reads the `X-Forwarded-Proto` header sent by nginx. Without this, Kestrel would see every request as plain HTTP and generate redirect loops.
