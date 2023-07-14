# BankID

Goal was to create a small POC thats able to display a QRCode that updates every second in the client using BankID's new V6 Api.

## Frontend

Start:

cd .\frontend\
yarn dev

## Backend

You know how to start it :)

## Resonering

Vi valde en client som ligger och pollar pga. följande anledningar

- Även om efter vi anropat /auth skulle få den nya QRCoden pushad till client varje sekund antingen via websockets eller long-polling så
måste klienten ändå ligga och polla /collect endpointen. För det är först vid det anropet som vi vet status på bankid transaktionen.

Så vi tänkte vi slår ihop det. Så /collect retunerar både statusen för transaktionen samt den nya QRCoden.

Visserligen hade det gått att lösa med två stycken long-polling/websockets också. 

- Kikade på flödet vid några andra sidors hantering av BankID V6. Dom vi tittade på låg också och pollade från clienten

- Det var enklare såhär :) 

## Kvar att göra

- Den pollanden timern i frontenden avslutas ej korrekt. Vi försöker avbryta timern när status är "complete" eller "failed".
Det är tekniskt möjligt att göra så, men vi la inte mer tid på att lösa det helt.

-