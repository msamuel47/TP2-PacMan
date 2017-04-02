using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;
namespace TP2PROF
{
    /// <summary>
    /// Classe d'une partie de Pacman.
    /// </summary>
    public class PacmanGame
    {
        //<nDionne>
        #region Propriétés
        /// <summary>
        /// Nombre de cases en largeur dans le jeu du Pacman
        /// </summary>
        public const int DEFAULT_GAME_WIDTH = 21;

        /// <summary>
        /// Nombre de cases en hauteur dans le jeu du Pacman
        /// </summary>
        public const int DEFAULT_GAME_HEIGHT = 22;

        /// <summary>
        /// Largeur de rendu d'un élément de jeu
        /// </summary>
        public const int DEFAULT_GAME_ELEMENT_HEIGHT = 20;

        /// <summary>
        /// Hauteur de rendu d'un élément de jeu
        /// </summary>
        public const int DEFAULT_GAME_ELEMENT_WIDTH = 20;
       
        /// <summary>
        /// La grille principale de jeu. Elle est créée dans la méthode LoadGrid
        /// </summary>
        public Grid grid = null;
        /// <summary>
        /// Nombre de fantômes présents dans le jeu
        /// </summary>
        public const int NB_GHOSTS = 4;
        /// <summary>
        /// Les 4 fantômes du jeu
        /// </summary>
        private Ghost[] ghosts;
        /// <summary>
        /// Le pacman du jeu
        /// </summary>
        private Pacman pacman;

        /// <summary>
        /// Durée d'activation d'une superpastille.
        /// </summary>
        private const int SUPERPILL_ACTIVATION_TIME = 5;
        int durationSuperPill;

        /// <summary>
        /// Accesseur permettant de savoir si une super pastille est active
        /// Propriété C#
        /// </summary>
        bool SuperPillActive = false;
        
        // Propriétés SFML pour l'affichage des pastilles et super-pastilles
        const float SMALL_PILL_RADIUS = DEFAULT_GAME_ELEMENT_HEIGHT / 8;
        const float SUPER_PILL_RADIUS = 2 * SMALL_PILL_RADIUS;
        CircleShape smallPillShape = new CircleShape(SMALL_PILL_RADIUS);
        CircleShape superPillShape = new CircleShape(SUPER_PILL_RADIUS);

        // Propriétés SFML pour l'affichage du labyrinthe
        Texture wallTexture = new Texture("Assets/Wall.bmp");
        Sprite wallSprite = null;
        #endregion

        /// <summary>
        /// Constructeur du jeu de Pacman
        /// </summary>
        public PacmanGame()
        {
            //Construction de la grille.
            grid = new Grid();
            //Initialisation des fantômes.
            ghosts = new Ghost[NB_GHOSTS];
            
            // Initialisation SFML
            smallPillShape.Origin = new Vector2f((float)-(DEFAULT_GAME_ELEMENT_WIDTH - SMALL_PILL_RADIUS) / 2, -(float)(DEFAULT_GAME_ELEMENT_HEIGHT - SMALL_PILL_RADIUS) / 2);
            superPillShape.Origin = new Vector2f((float)-(DEFAULT_GAME_ELEMENT_WIDTH - SUPER_PILL_RADIUS) / 2, -(float)(DEFAULT_GAME_ELEMENT_HEIGHT - SUPER_PILL_RADIUS) / 2);
            wallSprite = new Sprite(wallTexture);
        }
        /// <summary>
        /// Charge un fichier de labyrinthe.
        /// </summary>
        /// <param name="path">Le fichier contenant la description du labyrinthe de jeu</param>
        /// <returns>true si le chargement s'est bien effectué, false sinon</returns>
        public bool LoadGrid(string path)
        {
            bool retval = System.IO.File.Exists(path);
            if (retval)
            {
                string fileContent = System.IO.File.ReadAllText(path);
                grid.LoadFromMemory(fileContent);
                // Si le chargement s'est correctement effectué
                if (true == retval)
                {
                    // On parcourt la grille et, avec la méthode GetGridElementAt
                    // On trouve les positions où il y a des fantômes et la position du Pacman. On les retire de la grille en
                    //les "remplaçant"par les instances des classes correspondantes.
                    for (int i = 0; i < grid.Width; i++)
                    {
                        for (int j = 0; j < grid.Height; j++)
                        {
                            if (grid.GetGridElementAt(j, i) == PacmanElement.Ghost)
                            {
                                ghosts[Ghost.nbGhostCreated] = new Ghost(j, i);
                                grid.SetGridElementAt(j, i, 0);
                            }
                            else if (grid.GetGridElementAt(j, i) == PacmanElement.Pacman)
                            {
                                pacman = new Pacman(j, i);
                                grid.SetGridElementAt(j, i, 0);
                            }
                        }
                    }
                }
            }
            return retval;
        }
        /// <summary>
        /// Met à jour la logique de jeu
        /// </summary>
        /// <param name="key">La touche entrée par le joueur pour contrôle le pacman</param>
        /// <returns>EndGameResult.NotFinished si la partie est toujours en cours, EndGameResult.Win
        /// si le joueur a mangé toutes les pastilles ou EndGameResult.Losse si le joueur s'est fait
        /// mangé par un fantôme</returns>
        public EndGameResult Update(Keyboard.Key key)
        {            
            //Position du pacman, transformé en vecteur.
            Vector2i pacmanPosition = new Vector2i(pacman.Column, pacman.Row);
            // Déplacement du joueur
            if (key == Keyboard.Key.Left)
            {
                if (grid.GetGridElementAt(pacman.Row, pacman.Column - 1) != PacmanElement.Wall)
                {
                    pacman.Column--;
                }
            }
            else if (key == Keyboard.Key.Right)
            {
                if (grid.GetGridElementAt(pacman.Row, pacman.Column + 1) != PacmanElement.Wall)
                {
                    pacman.Column++;
                }
            }
            else if (key == Keyboard.Key.Up)
            {
                if (grid.GetGridElementAt(pacman.Row - 1, pacman.Column) != PacmanElement.Wall)
                {
                    pacman.Row--;
                }
            }
            else if (key == Keyboard.Key.Down)
            {
                if (grid.GetGridElementAt(pacman.Row + 1, pacman.Column) != PacmanElement.Wall)
                {
                    pacman.Row++;
                }
            }
            // Mise à jour des fantômes
            for (int i = 0; i < ghosts.Length; i++)
            {
                ghosts[i].Update(grid, pacmanPosition, SuperPillActive);
            }
            
            // Vérification du ramassage d'une pastille
            if (grid.GetGridElementAt(pacman.Row, pacman.Column) == PacmanElement.Pill)
            {
                grid.SetGridElementAt(pacman.Row, pacman.Column, 0);                
            }
            // Vérification de l'activation d'un superpill
            if (grid.GetGridElementAt(pacman.Row, pacman.Column) == PacmanElement.SuperPill)
            {
                durationSuperPill = 0;
                grid.SetGridElementAt(pacman.Row, pacman.Column, 0);
                SuperPillActive = true;
            }
            //Vérification de la durée d'un superpill.
            if (SuperPillActive)
            {                
                if (durationSuperPill == SUPERPILL_ACTIVATION_TIME*Application.TARGET_FPS)
                {
                    SuperPillActive = false;
                    durationSuperPill = 0;
                }
                else
                {
                    durationSuperPill++;
                }
                //Lorsque Pacman mange un fantôme, celui-ci retourne directement dans la cage.
                for (int i = 0; i < ghosts.Length; i++)
                {
                    Vector2i ghostPosition = new Vector2i(ghosts[i].Column, ghosts[i].Row);
                    if (pacmanPosition == ghostPosition)
                    {
                        ghosts[i].Column = grid.GhostCagePositionColumn;
                        ghosts[i].Row = grid.GhostCagePositionRow;
                    }
                }
            }            
            // Validations de fin de partie
            EndGameResult gameResult = EndGameResult.NotFinished;
            //Il reste des pills, la partie n'est pas finie.
            for(int i = 0; i < DEFAULT_GAME_ELEMENT_WIDTH; i++)
            {
                for(int j = 0; j < DEFAULT_GAME_ELEMENT_HEIGHT; j++)
                {
                    if (grid.GetGridElementAt(j, i) == PacmanElement.Pill || grid.GetGridElementAt(j, i) == PacmanElement.SuperPill)
                    {
                        gameResult = EndGameResult.NotFinished;
                    }
                }
            }
            //Un fantôme a atteint pacman, la partie est perdue.
            for (int i = 0; i < ghosts.Length; i++)
            {
                Vector2i ghostPosition = new Vector2i(ghosts[i].Column, ghosts[i].Row);
                if (pacmanPosition == ghostPosition&&SuperPillActive==false)
                {                    
                    gameResult = EndGameResult.Losse;                    
                }
            }
            //Il ne reste plus de pills, la partie est gagnée.
            int nbPillsRemaining = CountNbPillsRemaining();
            if (nbPillsRemaining == 0)
            {
                gameResult = EndGameResult.Win;
            }            
            return gameResult;            
        }
        /// <summary>
        /// Calcule le nombre de pastille non encore ramassées par le pacman
        /// </summary>
        /// <returns>Le nombre de pastille non encore ramassées</returns>
        private int CountNbPillsRemaining()
        {
            int nbPillsRemaining = 0;
            for (int i = 0; i < DEFAULT_GAME_WIDTH; i++)
            {
                for (int j = 0; j < DEFAULT_GAME_HEIGHT; j++)
                {
                    if (grid.GetGridElementAt(j, i) == PacmanElement.Pill || grid.GetGridElementAt(j, i) == PacmanElement.SuperPill)
                    {
                        nbPillsRemaining++;
                    }
                }
            }
            return nbPillsRemaining;
        }
        /// <summary>
        /// Dessine les éléments du jeu à l'écran
        /// </summary>
        /// <param name="window">Le contexte de rendu</param>
        public void Draw(RenderWindow window)
        {
            // PPOULIN
            for (int row = 0; row < grid.Height; row++)
            {
                for (int col = 0; col < grid.Width; col++)
                {
                    //Pastille régulière
                    if (grid.GetGridElementAt(row, col) == PacmanElement.Pill)
                    {
                        smallPillShape.Position = new Vector2f(col * DEFAULT_GAME_ELEMENT_WIDTH, row * DEFAULT_GAME_ELEMENT_HEIGHT);
                        window.Draw(smallPillShape);
                    }
                    // Super pastille
                    else if (grid.GetGridElementAt(row, col) == PacmanElement.SuperPill)
                    {

                        superPillShape.Radius = SUPER_PILL_RADIUS;
                        superPillShape.Position = new Vector2f(col * DEFAULT_GAME_ELEMENT_WIDTH, row * DEFAULT_GAME_ELEMENT_HEIGHT);
                        window.Draw(superPillShape);
                    }
                    // Mur
                    else if (grid.GetGridElementAt(row, col) == PacmanElement.Wall)
                    {
                        wallSprite.Position = new Vector2f(col * DEFAULT_GAME_ELEMENT_WIDTH, row * DEFAULT_GAME_ELEMENT_HEIGHT);
                        window.Draw(wallSprite);
                    }
                }
            }

            // Les 4 fantômes
            for (int i = 0; i < NB_GHOSTS; i++)
            {
                if (ghosts[i] != null)
                    ghosts[i].Draw(window, SuperPillActive);
            }

            // Le pacman
            if (null != pacman)
                pacman.Draw(window);

        }
        //</nDionne>
    }
  }
