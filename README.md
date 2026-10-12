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
  - « Nombre trop grand » (plus de 12 chiffres)
  - « Entrée invalide » quand on a tapé autre chose qu'un nombre dans l'écran

### Version 2

- `+/-` : change le signe du nombre affiché.
- `%` : divise le nombre affiché par 100 (`200 * 10 % =` donne `20`).
- `√` : racine carrée, avec le message « Racine négative » pour un nombre négatif.
- Mémoire : `MC` (vider), `MR` (afficher), `M+` (ajouter), `M-` (retirer). Un petit « M » s'affiche quand la mémoire est utilisée.
- Calculs à la suite : `2 + 3 * 4 =` donne `20` (de gauche à droite).
- Rappel du calcul en haut de l'écran (par exemple `12 +`, puis `12 + 3 =`).

## Utilisation et clavier

La calculatrice s'utilise à la souris. On peut aussi cliquer dans l'écran et taper un nombre au clavier, puis cliquer sur un opérateur ou sur `=`.

Exemple : cliquer sur `1`, `2`, `+`, `3`, `=` affiche `15`.

## Choix de conception

- `MainWindow.xaml` fait l'apparence, `MainWindow.xaml.cs` fait le comportement.
- Une fonction par opération : `Addition`, `Soustraction`, `Multiplication`, `Division`, `Pourcentage`, `Racine`, `Signe`.
- Pas de code recopié : les boutons appellent des fonctions communes (`AjouterChiffre`, `ChoisirOperateur`, `Calculer`, `Effacer`, `AfficherErreur`).
- L'opérateur n'est pas écrit sur l'écran, il est gardé dans la variable `operation`.
- Le choix de l'opération se fait avec des `if / else if` dans `Calculer`.
- Les nombres sont des `double` pour avoir les décimales. Le texte de l'écran est converti avec `double.Parse`, et les nombres sont affichés avec `ToString()`.
- Les erreurs :
  - des `if` pour ce qu'on peut prévoir (division par 0, racine d'un nombre négatif, écran vide) ;
  - un `try / catch` dans `Lire` pour un texte qui n'est pas un nombre ;
  - une variable `bool erreur` qui bloque les boutons tant qu'un message est affiché.
- Deux autres variables `bool` : `nouveau` (le prochain chiffre efface l'écran) et `virgule` (le nombre a déjà une virgule).
- Le style XAML est dans `Grid.Resources` : un style pour tous les boutons, avec un `ControlTemplate` pour les coins arrondis et des `Trigger` pour le survol et le clic. Les boutons qui ne sont pas des chiffres changent juste leur couleur avec `Background`.

## Tests

| Test | Résultat attendu |
| --- | --- |
| `12 + 3 =` | `15` |
| `7 * 8 =` | `56` |
| `10 / 4 =` | `2,5` |
| `9 / 0 =` | « Division par 0 » |
| `5 , ,` | `5,` |
| `C` | Écran vide |
| `5 +/-` | `-5` |
| `9 √` | `3` |
| `1 +/- √` | « Racine négative » |
| `2 + 3 * 4 =` | `20` |
| `0,1 + 0,2 =` | `0,3` |
| `12 * 3 = M+ 20 * 2 = M+ MR` | `76` |
| Taper `abc` dans l'écran puis `+` | « Entrée invalide » |

## Limites

- Pas de priorité des opérations : `2 + 3 * 4` donne `20` et pas `14`.
- `1 / 3 =` puis `* 3 =` donne un nombre très proche de 1 au lieu de 1, car le résultat est relu sur l'écran.
- La virgule dépend des réglages de Windows : il doit être réglé en français.
- Pour `+/-`, `%` et `√`, il faut taper le nombre avant de cliquer sur le bouton.
- Si on tape au clavier juste après un résultat, le chiffre s'ajoute derrière le résultat.
- Pas d'historique.
