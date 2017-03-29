﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
namespace TP2PROF
{
  public class Ghost
  {
    //<nDionne>
    #region Propriétés
    /// <summary>
        /// Position du fantôme
        /// </summary>
    private Vector2i position;
    /// <summary>
        /// Accesseur de la position en colonne
        /// Propriété C#
        /// </summary>
    public int Column
        {
            get { return position.X; }
            set
            {
                if (value >= 0)
                {
                    position.X = value;
                }
            }
        }
    /// <summary>
       /// Accesseur de la position en ligne
       /// Propriété C#
       /// </summary>
    public int Row
        {
            get { return position.Y; }
            set
            {
                if (value >= 0)
                {
                    position.Y = value;
                }
            }
        }
    /// <summary>
        /// Indique si le fantôme a été mangé par le pacman sans être
        /// retourné dans sa cage pour se régénérer
        /// </summary>
    private bool isWeak;
    /// <summary>
    /// Accesseur de la propriété isWeak
    /// Propriété C#
    /// </summary>
    public bool IsWeak
        { 
            get { return isWeak; }
        }
    /// <summary>
    /// Propriétés SFML pour l'affichage
    /// </summary>
    Texture ghostTextureNormal = new Texture("Assets/Ghost.bmp");
    Texture ghostTextureScared = new Texture("Assets/GhostScared.bmp");
    Texture ghostTextureWeak = new Texture("Assets/GhostWeak.bmp");
    Sprite ghostSprite = null;
    /// <summary>
    /// Identifiant du fantôme (entre 0 et 3 inclusivement) pour déterminer
    /// la couleur par défaut du fantôme dans la méthode Draw.
    /// </summary>
    private int ghostId = 0;
    /// <summary>
    /// Délai pour que le fantôme se mette en mouvement au début
    /// </summary>
        
    /// <summary>
    /// Pour l'affichage SFML
    /// </summary>
    static Color[] ghostColors = new Color[] { Color.Red, new Color(255, 192, 203), new Color(137, 207, 240), new Color(255, 127, 80) };    
    /// <summary>
    /// Compteur du nombre d'instances de fantômes
    /// </summary>
    public static int nbGhostCreated = 0;
    static Random rnd = new Random();
    #endregion

    /// <summary>
    /// Constructeur d'une instance de Ghost.
    /// </summary>
    /// <param name="row">Représente la position en rangée à affecter au fantôme(>=0).</param>
    /// <param name="column">Représente la position en colonne à affecter au fantôme(>=0).</param>
    public Ghost(int row, int column)
    {
       // Affectation de la position du fantôme  
       // Ne pas oublier de lancer une exception si les paramètres sont invalides
       try
            { 
                position.Y = row;
                position.X = column;
            }
       catch(IndexOutOfRangeException exception )
            {
                Console.WriteLine(exception.ToString());
                throw;
            }

       // Affectation de la propriété ghostId.
       // Quelle serait la meilleure "valeur" à affecter ici???
       ghostId = 0;

            // Incrémenter ici la propriété servant à compter le nombre de fantômes créés
            // jusqu'à date
      nbGhostCreated++;

      // Initialisation SFML
      ghostSprite = new Sprite(ghostTextureNormal);
      ghostSprite.Origin = new Vector2f(ghostTextureNormal.Size.X / 2, ghostTextureNormal.Size.Y / 2);
    }
    /// <summary>
    /// Déplace le fantôme selon une direction donnée.
    /// </summary>
    /// <param name="direction">Direction dans laquelle on veut déplacer le fantôme</param>
    /// <param name="grid">Grille de référence. Utilisée pour ne pas que le fantôme passe au travers des murs</param>
    public void Move(Direction direction, Grid grid)
        {
            if (direction == Direction.East&&grid.GetGridElementAt(Row,Column+1)!=PacmanElement.Wall)
            {
                Column++;
            }
            else if (direction == Direction.West && grid.GetGridElementAt(Row, Column - 1) != PacmanElement.Wall)
            {
                Column--;
            }
            else if (direction == Direction.North && grid.GetGridElementAt(Row - 1, Column) != PacmanElement.Wall)
            {
                Row--;
            }
            else if (direction == Direction.South && grid.GetGridElementAt(Row + 1, Column) != PacmanElement.Wall)
            {
                Row++;
            }
        }
    /// <summary>
    /// Affiche le fantôme dans la fenêtre de rendu.
    /// </summary>
    /// <param name="window">Fenêtre de rendu</param>
    /// <param name="isSuperPillActive">true si une super-pastille est active, false sinon</param>
    public void Draw(RenderWindow window, bool isSuperPillActive)
    {
      // Mise à jour de la texture du fantôme selon l'état du fantôme
      if (isSuperPillActive)
      {
        // État "effrayé", i.-e. le pacman a mangé une superpastille
        ghostSprite.Texture = ghostTextureWeak;
        ghostSprite.Color = Color.White;
      }
      else
      {
        // État "normal"
        ghostSprite.Texture = ghostTextureNormal;
        ghostSprite.Color = ghostColors[ghostId];
      }

            // ppoulin
            // A décommenter lorsqu'il sera possible d'accéder aux propriétés Column et Row
            // du fantôme
            ghostSprite.Position = new Vector2f(PacmanGame.DEFAULT_GAME_ELEMENT_WIDTH * Column,
                                                PacmanGame.DEFAULT_GAME_ELEMENT_HEIGHT * Row)
                                   + ghostSprite.Origin;
            window.Draw(ghostSprite);
    }
    /// <summary>
    /// Met à jour la position du fantôme
    /// </summary>
    /// <param name="grid">Grille de référence. Utilisée pour ne pas que le fantôme passe au travers des murs</param>
    /// <param name="pacmanPosition"></param>
    /// <param name="isSuperPillActive"></param>
    public void Update(Grid grid, Vector2i pacmanPosition, bool isSuperPillActive)
    {
            if (pacmanPosition == position&&isSuperPillActive==true)
            {
                isWeak = true;
            }
            if(isSuperPillActive==false)
            {
                Direction firstDirection = PathFinder.FindShortestPath(grid, Column, Row, pacmanPosition.X, pacmanPosition.Y);
                Move(firstDirection, grid);
            }            
    }
  }
}
