using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace TP2PROF
{
    public class Grid
    {
        //<SamuelV>
        /// <summary>
        /// Grille logique du jeu.
        /// Tableau 2D de PacmanElement
        /// </summary>
        private pacmanElement[,] elements = null;

        //<SamuelV>
        /// <summary>
        /// Position de la cage des fantômes
        /// </summary>
        public Vector2i ghostCagePosition;

        // A compléter

        //<SamuelV>
        /// <summary>
        /// Accesseur du numéro de la ligne où se trouve la cage à fantômes
        /// Propriété C#
        /// </summary>
        public int GhostCagePositionRow
        {
            get { return ghostCagePosition.Y; }
        }


        //<SamuelV>
        /// <summary>
        /// Accesseur du numéro de la colonne où se trouve la cage à fantômes
        /// Propriété C#
        /// </summary>
        public int GhostCagePositionColumn
        {
            get { return ghostCagePosition.X; }
        }
        //<SamuelV>
        /// <summary>
        /// Position originale du pacman
        /// </summary>
        private Vector2i pacmanOriginalPosition;
        
        //<SamuelV>
        /// <summary>
        /// Accesseur du numéro de la ligne où se trouve le pacman au début
        /// Propriété c#
        /// </summary>
        public int PacmanOriginalPositionRow
        {
            get { return pacmanOriginalPosition.Y; }
        }
        

        //<SamuelV>
        /// <summary>
        /// Accesseur du numéro de la colonne où se trouve le pacman au début
        /// Propriété C#
        /// </summary>
        public int PacmanOriginalPositionColumn
        {
            get { return pacmanOriginalPosition.X; }
        }

        /// <summary>
        /// Accesseur de la hauteur
        /// Propriété C#
        /// </summary>
        public int Height
        {
            get { return elements.GetLength(1); }
        }
        // A compléter
        /// <summary>
        /// Accesseur de la largeur
        /// Propriété C#
        /// </summary>
        public int Width
        {
            get { return elements.GetLength(0); }
        }
        
        //<SamuelV>
        /// <summary>
        /// Constructeur sans paramètre
        /// </summary>
        // A compléter
        /// <summary>
        /// Charge un niveau à partir d'une chaine de caractères en mémoire.
        /// Voir l'énoncé du travail pour le format de la chaîne.
        /// </summary>
        /// <param name="content"> Le contenu du niveau en mémoire</param>
        /// <returns>true si le chargement est correct, false sinon</returns>
        public bool LoadFromMemory(string content)
        {
            int pacmanCount = 0;
            int ghostCageCount = 0;
            bool retval = true;
            int[] nombreAccepter = new[] {0, 1, 2, 3, 4, 5, 6};
            int[,] converted2DArray = null;

            // A compléter selon les spécifications du travail
            //Utilisation de la technologie LINQ pour charger le tableau
            string[][] TableauDeTableaux = content.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim(new char[] { '\r', '\n', ' ' }).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
            //Valide si les dimensions du tableau sont valides
            if (TableauDeTableaux.Length != 22 && TableauDeTableaux[0].Length != 21)
            {
                retval = false;
            }
            else
            {
                //Conversion du tableau de tableau en tableau 2D de nombre + échec si tableau contient un mauvais
                // caractère
                converted2DArray = new int[TableauDeTableaux.Length,TableauDeTableaux[0].Length];
                for (int i = 0; i < converted2DArray.GetLength(0); i++)
                {
                    for (int j = 0; j < converted2DArray.GetLength(1) ; j++)
                    {
                        retval = int.TryParse(TableauDeTableaux[i][j], out converted2DArray[i, j]);
                        for (int k = 0; k < nombreAccepter.Length; k++)
                        {
                            if (converted2DArray[i, j] != nombreAccepter[k])
                            {
                                retval = false;
                                converted2DArray[i, j] = -1;
                            }
                        }
                        if (converted2DArray[i, j] == 3)
                        {
                            pacmanCount++;
                        }
                        if (converted2DArray[i, j] == 6)
                        {
                            ghostCageCount++;
                        }
                        
                        if (retval == false)
                        {
                            converted2DArray[i, j] = -1;
                        }
                    }
                    
                }
                if (pacmanCount != 1 || ghostCageCount != 1)
                {
                    retval = false;
                }

            }
            return retval;
        }

        /// <summary>
        /// Retourne l'élément à la position spécifiée
        /// </summary>
        /// <param name="row">La ligne</param>
        /// <param name="column">La colonne</param>
        /// <returns>L'élément à la position spécifiée</returns>
        // A compléter


        /// <summary>
        /// Modifie le contenu du tableau à la position spécifiée
        /// </summary>
        /// <param name="row">La ligne</param>
        /// <param name="column">La colonne</param>
        /// <param name="element">Le nouvel élément à spécifier</param>
        // A compléter
    }
}