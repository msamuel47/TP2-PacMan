using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SFML.System;
using SFML.Window;

namespace TP2PROF
{
    public class Grid
    {
        //<SamuelV>
        /// <summary>
        /// Grille logique du jeu.
        /// Tableau 2D de PacmanElement
        /// </summary>
        private PacmanElement[,] elements = null;

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
            private set { pacmanOriginalPosition.Y = value; }
        }
        
        

        //<SamuelV>
        /// <summary>
        /// Accesseur du numéro de la colonne où se trouve le pacman au début
        /// Propriété C#
        /// </summary>
        public int PacmanOriginalPositionColumn
        {
            get { return pacmanOriginalPosition.X; }
            private  set { pacmanOriginalPosition.X = value; }
        }
        //<SamuelV>
        /// <summary>
        /// Accesseur de la hauteur
        /// Propriété C#
        /// </summary>
        public int Height
        {
            get { return PacmanGame.DEFAULT_GAME_HEIGHT; }
        }
        //<SamuelV>
        /// <summary>
        /// Accesseur de la largeur
        /// Propriété C#
        /// </summary>
        public int Width
        {
            get { return PacmanGame.DEFAULT_GAME_WIDTH; }
        }
        
        //<SamuelV>
        /// <summary>
        /// Constructeur sans paramètre
        /// </summary>
        public Grid()
        {
            elements = new PacmanElement[22, 21];
        }
       
        //<SamuelV>
        /// <summary>
        /// Charge un niveau à partir d'une chaine de caractères en mémoire.
        /// Voir l'énoncé du travail pour le format de la chaîne.
        /// </summary>
        /// <param name="content"> Le contenu du niveau en mémoire</param>
        /// <returns>true si le chargement est correct, false sinon</returns>
        public bool LoadFromMemory(string content)
        {
            int[,] converted2DArray = null;
            int pacmanCount = 0;
            int ghostCageCount = 0;
            bool retval = true;
            int[] nombreAccepter = new[] {0, 1, 2, 3, 4, 5, 6};
            if (String.IsNullOrEmpty(content))
            {
                return false;
            }
            //Utilisation de la technologie LINQ pour charger le tableau
            string[][] TableauDeTableaux =
                content.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(
                        x =>
                            x.Trim(new char[] {'\r', '\n', ' '})
                                .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                    .ToArray();
            //Valide si les dimensions du tableau sont valides
            if (TableauDeTableaux.Length != 22)
            {
                retval = false;
            }
            for (int i = 0; i < TableauDeTableaux.Length; i++)
            {
                if (TableauDeTableaux[i].Length != 21)
                {
                    retval = false;
                }
            }
            if (retval == false)
            {
                
            }
        else

        {
                //Conversion du tableau de tableau en tableau 2D de nombre + échec si tableau contient un mauvais
                // caractère
                converted2DArray = new int[TableauDeTableaux.Length,TableauDeTableaux[0].Length];
                bool nombreValide = false;
                for (int i = 0; i < converted2DArray.GetLength(0); i++)
                {
                    for (int j = 0; j < converted2DArray.GetLength(1) ; j++)
                    {
                        retval = int.TryParse(TableauDeTableaux[i][j], out converted2DArray[i, j]);
                        nombreValide = false;
                        for (int k = 0; k < nombreAccepter.Length; k++)
                        {
                            
                            if (converted2DArray[i, j] == nombreAccepter[k])
                            {
                                nombreValide = true;
                                
                            }
                        }
                        if (nombreValide != true)
                        {
                            retval = false;
                            converted2DArray[i, j] = -1;
                        }
                        if (converted2DArray[i, j] == 3)
                        {
                            PacmanOriginalPositionColumn = j;
                            PacmanOriginalPositionRow = i;
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
            if (retval)
            {
                
                for (int i = 0; i < elements.GetLength(0); i++)
                {
                    for (int j = 0; j < elements.GetLength(1); j++)
                    {
                        SetGridElementAt(i,j,converted2DArray[i,j]);
                    }
                }
            }
            return retval;
        }
        //</SamuelV>
        

        //<SamuelV>
        /// <summary>
        /// Retourne l'élément à la position spécifiée
        /// </summary>
        /// <param name="row">La ligne</param>
        /// <param name="column">La colonne</param>
        /// <returns>L'élément à la position spécifiée</returns>
        public PacmanElement GetGridElementAt(int row,int column)
        {
            if (row < 0 || row > PacmanGame.DEFAULT_GAME_HEIGHT || column < 0 || column > PacmanGame.DEFAULT_GAME_WIDTH)
            {
                throw new ArgumentOutOfRangeException("Les paramètres sont en dehors de la grille de jeu");
            }
                return elements[row, column];
            
            
        }
        //<SamuelV>
        public void SetGridElementAt(int row, int column, int number)
        {
            
            if (row<0||row>PacmanGame.DEFAULT_GAME_HEIGHT||column<0||column>PacmanGame.DEFAULT_GAME_WIDTH)
            {
                throw new ArgumentOutOfRangeException("Les paramètres sont en dehors de la grille de jeu");
            }
            if (number == 0)
            {
                elements[row , column] = PacmanElement.None;
            }
            if (number == 1)
            {
                elements[row, column] = PacmanElement.Wall;
            }
            if (number == 2)
            {
                elements[row, column] = PacmanElement.Ghost;
            }
            if (number == 3)
            {
                elements[row, column] = PacmanElement.Pacman;
            }
            if (number == 4)
            {
                elements[row, column] = PacmanElement.Pill;
            }
            if (number == 5)
            {
                elements[row, column] = PacmanElement.SuperPill;
            }
            if (number == 6)
            {
                elements[row, column] = PacmanElement.GhostCage;
            }
        }


        /// <summary>
        /// Modifie le contenu du tableau à la position spécifiée
        /// </summary>
        /// <param name="row">La ligne</param>
        /// <param name="column">La colonne</param>
        /// <param name="element">Le nouvel élément à spécifier</param>
        // A compléter
    }
}