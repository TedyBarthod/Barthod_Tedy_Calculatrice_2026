# Calculatrice WPF

Projet de TP de BTS CIEL 1 : une calculatrice en C# avec une interface WPF (XAML).

Auteur : Tedy Barthod

## Lancer le projet

1. Ouvrir `Barthod_Tedy_Calculatrice_2026.slnx` dans Visual Studio.
2. Appuyer sur `F5` (ou « Démarrer »).

Le projet utilise le .NET Framework 4.7.2.

## Fonctionnalités

### Version 1

- Saisie des nombres avec les boutons `0` à `9`.
- Nombres à virgule, avec la virgule française (`2,5`). Une deuxième virgule est ignorée.
- Les quatre opérations : `+`, `-`, `*`, `/`.
- `=` pour calculer et `C` pour tout effacer.
- Messages d'erreur à la place d'un plantage :
  - « Division par 0 »
  - « Nombre trop grand » (dépassement de la capacité d'un `double`)
  - « Entrée invalide » (texte qui n'est pas un nombre, géré avec un `try / catch`)

### Version 2

- `+/-` : change le signe du nombre affiché.
- `%` : divise le nombre affiché par 100 (`200 * 10 % =` donne `20`).
- `√` : racine carrée, avec le message « Racine négative » pour un nombre négatif.
- Mémoire : `MC` (vider), `MR` (afficher), `M+` (ajouter), `M-` (retirer). Un petit « M » s'affiche quand la mémoire est utilisée.
- Calculs à la suite : `2 + 3 * 4 =` donne `20` (calcul de gauche à droite).
- Rappel du calcul en cours en haut de l'écran (par exemple `12 +`, puis `12 + 3 =`).

## Utilisation et raccourcis clavier

La calculatrice s'utilise à la souris : il n'y a pas de raccourci clavier. L'écran est en lecture seule, on ne peut écrire qu'avec les boutons.

Exemple : cliquer sur `1`, `2`, `+`, `3`, `=` affiche `15`.

## Choix de conception

- **XAML et C# séparés** : `MainWindow.xaml` décrit l'apparence, `MainWindow.xaml.cs` décrit le comportement.
- **Une fonction par opération** : `Addition`, `Soustraction`, `Multiplication`, `Division`, `Pourcentage`, `Racine`, `Signe`.
- **Pas de code dupliqué** : les boutons appellent des fonctions communes (`AjouterChiffre`, `ChoisirOperateur`, `Calculer`, `Effacer`, `AfficherErreur`).
- **L'opérateur n'est pas écrit sur l'écran** : il est gardé dans la variable `operation`. L'écran contient seulement le nombre en cours.
- **Choix de l'opération avec `if / else if`** dans la fonction `Calculer`.
- **Nombres en `double`** pour avoir les décimales. `CultureInfo("fr-FR")` gère la virgule, et le format `G12` évite d'afficher `0,30000000000000004`.
- **Gestion des erreurs** :
  - des `if` pour les erreurs qu'on peut prévoir (division par 0, racine d'un nombre négatif, écran vide) ;
  - un `try / catch` dans la fonction `Lire`, le seul endroit où le texte de l'écran est converti en nombre ;
  - une variable `erreur` qui bloque les boutons tant qu'un message est affiché (`C` ou un chiffre repart de zéro).
- **Boucle `for`** dans `ContientVirgule` pour chercher une virgule dans le texte.
- **Styles XAML** dans `Grid.Resources` : un style pour tous les boutons, des styles avec `x:Key` pour les opérateurs, `=`, `C`, les fonctions et la mémoire, un `ControlTemplate` pour les coins arrondis, et des `Trigger` pour le survol et le clic.

## Tests

| Test | Résultat attendu |
| --- | --- |
| `12 + 3 =` | `15` |
| `7 * 8 =` | `56` |
| `10 / 4 =` | `2,5` |
| `9 / 0 =` | « Division par 0 » |
| `5 , ,` | `5,` (la deuxième virgule est ignorée) |
| `C` | Écran vide |
| `5 +/-` | `-5` |
| `9 √` | `3` |
| `1 +/- √` | « Racine négative » |
| `2 + 3 * 4 =` | `20` |
| `0,1 + 0,2 =` | `0,3` |
| `12 * 3 = M+ 20 * 2 = M+ MR` | `76` |

## Limites connues

- Pas de priorité des opérations : `2 + 3 * 4` donne `20` et pas `14`.
- Après `=`, le résultat est relu sur l'écran (12 chiffres), donc `1 / 3 =` puis `* 3 =` donne `0,999999999999`.
- Pour `+/-`, `%` et `√`, il faut taper le nombre avant de cliquer sur le bouton.
- Pas de saisie au clavier ni d'historique.
