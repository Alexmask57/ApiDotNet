# ApiDotNet

## Lancer le projet

- Démarrer tous les services :

    ```shell
    docker-compose up -d
    ```
- Initialiser la BDD MySql :
    ```shell
    cd ApiDotNet
    dotnet ef migrations add initialMigration
    dotnet ef database update
    ```
- Créer un user ayant le rôle administrateur
  Récupérer l'id du rôle "Administrateur" dans la table

La base de données est prête à être utilisée !

## Gitflow

```mermaid
sequenceDiagram
    participant dotcom
    participant iframe
    participant viewscreen
    dotcom->>iframe: loads html w/ iframe url
    iframe->>viewscreen: request template
    viewscreen->>iframe: html & javascript
    iframe->>dotcom: iframe ready
    dotcom->>iframe: set mermaid data on iframe
    iframe->>iframe: render mermaid
```
