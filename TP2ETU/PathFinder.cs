using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2PROF
{
  public static class PathFinder
  {
        
        //<SamuelV>
        /// <summary>
        /// Initialise le tableau des coûts de déplacements, Le tableau est 
        /// initialisé à int.MaxValue partout sauf à l'endroit où se trouve le
        /// fantôme où le tableau est initialisé à 0.
        /// </summary>
        /// <param name="aGrid">La grille du jeu: pour connaitre les dimensions attendues</param>
        /// <param name="fromX">La position du fantome en colonne</param>
        /// <param name="fromY">La position du fantome en ligne</param>
        /// <returns>Le tableau initialisé correctement</returns>
        public static int[,] InitCosts(Grid aGrid, int fromX, int fromY)
        {
            int[,] pathFinderGridInit = new int[aGrid.Height,aGrid.Width];
            for (int i = 0; i < aGrid.Height; i++)
            {
                for (int j = 0; j < aGrid.Width; j++)
                {
                    if (i == fromY && j == fromX)
                    {
                        pathFinderGridInit[i, j] = 0;
                    }
                    else
                    {
                        pathFinderGridInit[i, j] = int.MaxValue;
                    }
                }
            }
            return pathFinderGridInit;
        }
        //</SamuelV>
        /// <summary>
        /// Détermine le premier déplacement nécessaire pour déplacer un objet de la position (fromX, fromY)
        /// vers la position (toX, toY). 
        /// <param name="aGrid">La grille du jeu: pour connaitre les positions des murs</param>
        /// <param name="fromX">La position de départ en colonne</param>
        /// <param name="fromY">La position de départ en ligne</param>
        /// <param name="toX">La position cible en colonne</param>
        /// <param name="toY">La position cible en ligne</param>
        /// <remark>Typiquement, la position (fromX, fromY) est celle du fantôme tandis 
        /// que la position (toX, toY) est celle du pacman.</remark>
        /// <returns>La direction dans laquelle on doit aller. Direction.None si l'on
        /// est déjà rendu ou Direction.Undefined s'il est impossible d'atteindre la cible</returns>
        /// </summary>
        //<nDionne>
        public static Direction FindShortestPath(Grid aGrid, int fromX, int fromY, int toX, int toY)
        {
            int[,] initialCosts = InitCosts(aGrid, fromX, fromY);

            ComputeCosts(aGrid, fromX, fromY, toX, toY, initialCosts);

            return RecurseFindDirection(initialCosts, toX, toY, fromX, fromY);
        }
        //</nDionne>
        //<SamuelV>
        /// <summary>
        /// Calcule le nombre de déplacements requis pour aller de la position (fromX, fromY)
        /// vers la position (toX, toY). 
        /// <param name="aGrid">La grille du jeu: pour connaitre les positions des murs</param>
        /// <param name="fromX">La position de départ en colonne</param>
        /// <param name="fromY">La position de départ en ligne</param>
        /// <param name="toX">La position cible en colonne</param>
        /// <param name="toY">La position cible en ligne</param>
        /// <param name="costs">Le tableau des coûts à remplir</param>
        /// <remark>Typiquement, la position (fromX, fromY) est celle du fantôme tandis 
        /// que la position (toX, toY) est celle du pacman.</remark>
        /// <remark>Cette méthode est récursive</remark>
        /// </summary>
        public static void ComputeCosts(Grid aGrid, int fromX, int fromY, int toX, int toY, int[,] costs)
        {
            if (fromX != 0)
            {
                if (costs[fromY, fromX - 1] > costs[fromY, fromX] + 1)
                {
                    if (aGrid.GetGridElementAt(fromY, fromX - 1) != PacmanElement.Wall)
                    {
                        costs[fromY, fromX - 1] = costs[fromY, fromX] + 1;
                        ComputeCosts(aGrid , fromX - 1 , fromY , 0 , 0 , costs);
                    }
                }
            }
            if (fromX != aGrid.Width - 1)
            {
                if (costs[fromY, fromX + 1] > costs[fromY, fromX] + 1)
                {
                    if (aGrid.GetGridElementAt(fromY, fromX + 1) != PacmanElement.Wall)
                    {
                        costs[fromY, fromX + 1] = costs[fromY, fromX] + 1;
                        ComputeCosts(aGrid, fromX + 1, fromY, 0, 0, costs);
                    }
                }
            }
            if (fromY != 0)
            {
                if (costs[fromY - 1, fromX] > costs[fromY, fromX] + 1)
                {
                    if (aGrid.GetGridElementAt(fromY - 1, fromX) != PacmanElement.Wall)
                    {
                        costs[fromY - 1, fromX] = costs[fromY, fromX] + 1;
                        ComputeCosts(aGrid, fromX, fromY - 1, 0, 0, costs);
                    }
                }
            }
            if (fromY != aGrid.Width - 1)
            {
                if (costs[fromY + 1, fromX] > costs[fromY, fromX] + 1)
                {
                    if (aGrid.GetGridElementAt(fromY + 1, fromX) != PacmanElement.Wall)
                    {
                        costs[fromY + 1, fromX] = costs[fromY, fromX] + 1;
                        ComputeCosts(aGrid, fromX, fromY + 1, 0, 0, costs);
                    }
                }
            }
        }
        //</SamuelV>
        //<nDionne>
        /// <summary>
        /// Parcourt le tableau de coûts pour trouver le premier déplacement requis pour aller de la position (fromX, fromY)
        /// vers la position (toX, toY). 
        /// <param name="costs">Le tableau des coûts prédédemment calculés</param>
        /// <param name="targetX">La position cible en colonne</param>
        /// <param name="targetY">La position cible en ligne</param>
        /// <param name="fromX">La position de départ en colonne</param>
        /// <param name="fromY">La position de départ en ligne</param>
        /// <remark>Typiquement, la position (targetX, targetY) est celle du pacman.</remark>
        /// <remark>Cette méthode est récursive</remark>
        /// </summary>
        /// <returns>La direction dans laquelle on doit aller. Direction.None si l'on
        /// est déjà rendu ou Direction.Undefined s'il est impossible d'atteindre la cible</returns>
        public static Direction RecurseFindDirection(int[,] costs, int targetX, int targetY, int fromX, int fromY)
        {
            //Déplacement vers le sud...
            //On vérifie si la case au nord ne rencontre pas de murs et fait partie des dimensions de la grille du jeu.
            if (costs[targetY - 1, targetX] != int.MaxValue && targetY - 1 >= 0)
            {
                //On vérifie si la case au nord correspondrait à un prochain déplacement logique.
                if (costs[targetY - 1, targetX] == costs[targetY, targetX] - 1)
                {
                    //On vérifie si la case au nord correspondrait au fantôme. La première direction est donc trouvée.
                    if (costs[targetY - 1, targetX] == costs[fromY, fromX])
                    {
                        return Direction.South;
                    }
                    //Sinon on refait appel à la méthode jusqu'à ce qu'on se situe à une case du départ (cases[fromY,fromX]).
                    else
                    {
                        return RecurseFindDirection(costs, targetX, targetY - 1, fromX, fromY);
                    }
                }
                //Sinon, on vérifie si la case du fantôme est déja trouvée. On retourne "aucune" direction.
                else if (costs[targetY, targetX] == costs[fromY, fromX])
                {
                    return Direction.None;
                }
            }
            //Mêmes conditions, pour le nord...
            if (costs[targetY + 1, targetX] != int.MaxValue && targetY + 1 < 22)
            {
                if (costs[targetY + 1, targetX] == costs[targetY, targetX] - 1)
                {
                    if (costs[targetY + 1, targetX] == 0)
                    {
                        return Direction.North;
                    }
                    else
                    {
                        return RecurseFindDirection(costs, targetX, targetY + 1, fromX, fromY);
                    }
                }
                else if (costs[targetY, targetX] == costs[fromY, fromX])
                {
                    return Direction.None;
                }
            }
            //Mêmes conditions, pour l'est...
            if (costs[targetY, targetX - 1] != int.MaxValue && targetX - 1 >= 0)
            {
                if (costs[targetY, targetX - 1] == costs[targetY, targetX] - 1)
                {
                    if (costs[targetY, targetX - 1] == 0)
                    {
                        return Direction.East;
                    }
                    else
                    {
                        return RecurseFindDirection(costs, targetX - 1, targetY, fromX, fromY);
                    }
                }
                else if (costs[targetY, targetX] == costs[fromY, fromX])
                {
                    return Direction.None;
                }
            }
            //Mêmes conditions, pour l'ouest...
            if (costs[targetY, targetX + 1] != int.MaxValue && targetX + 1 < 22)
            {
                if (costs[targetY, targetX + 1] == costs[targetY, targetX] - 1)
                {
                    if (costs[targetY, targetX + 1] == 0)
                    {
                        return Direction.West;
                    }
                    else
                    {
                        return RecurseFindDirection(costs, targetX + 1, targetY, fromX, fromY);
                    }
                }
                else if (costs[targetY, targetX] == costs[fromY, fromX])
                {
                    return Direction.None;
                }
            }
            //Si aucune direction ne respecte les conditions ci-dessus, la direction est indéfinie, et le fantôme ne peut donc pas bouger.
            return Direction.Undefined;
        }
        //</nDionne>   
    }

}
