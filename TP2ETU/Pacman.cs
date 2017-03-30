using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
namespace TP2PROF
{
  public class Pacman
  {
      //<SamuelV>
      /// <summary>
      /// Position du pacman
      private Vector2i position;

      /// </summary>

      /// <summary>
      /// Accesseur de la position en colonne
      /// Propriété C#
      /// </summary>
      public int Column
      {
            get { return position.X; }
            set { position.X = value; }
      }

      /// <summary>
      /// Accesseur de la position en ligne
      /// Propriété C#
      /// </summary>
      public int Row
      {
          get { return position.Y; }
          set { position.Y = value; }
      }


    
    // Propriétés SFML pour l'affichage
    Texture pacmanTexture = new Texture("Assets/Pacman.bmp");
    Sprite pacmanSprite = null;

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="row">Ligne de départ du pacman</param>
    /// <param name="column">Colonne de départ du pacman</param>
    public Pacman(int row, int column)
    {
        if (row <= 0 || column <= 0)
        {
            throw new FormatException("Les paramètres entrés dans le constructeur Pacman est invalide ...");
          
        }
            position.Y = row;
            position.X = column;
        
        
        // Ne pas oublier de lancer une exception si les paramètres sont invalides
        
      // Initialisation des propriétés SFML
      pacmanSprite = new Sprite(pacmanTexture);
      pacmanSprite.Origin = new Vector2f(pacmanTexture.Size.X/2, pacmanTexture.Size.Y / 2);
    }

      /// <summary>
      /// Déplace le pacman selon une direction donnée.
      /// </summary>
      /// <param name="direction">Direction dans laquelle on veut déplacer le pacman</param>
      /// <param name="grid">Grille de référence. Utilisée pour ne pas que le pacman passe au travers des murs</param>
      public void Move(Direction direction, Grid grid)
      {
          if (direction == Direction.West)
          {
              if (Column - 1 > 0)
              {
                  if (grid.GetGridElementAt(Row, Column - 1) != PacmanElement.Wall)
                  {
                      grid.SetGridElementAt(Row, Column, 0);
                      Column--;
                      grid.SetGridElementAt(Row, Column, 3);
                  }
              }
          }
          else if (direction == Direction.East)
          {
              if (Column < grid.Width - 1)
              {
                  if (grid.GetGridElementAt(Row, Column + 1) != PacmanElement.Wall)
                  {
                      grid.SetGridElementAt(Row, Column, 0);
                      Column++;
                      grid.SetGridElementAt(Row, Column, 3);
                  }
              }
          }
          else if (direction == Direction.North)
          {
              if (Row - 1 < 0)
              {
                  if (grid.GetGridElementAt(Row - 1, Column) != PacmanElement.Wall)
                  {
                      grid.SetGridElementAt(Row, Column, 0);
                      Row--;
                      grid.SetGridElementAt(Row, Column, 3);
                  }
              }
          }
          else if (direction == Direction.South)
          {
              if (Row < grid.Width - 1)
              {
                  if (grid.GetGridElementAt(Row + 1, Column) != PacmanElement.Wall)
                  {
                      grid.SetGridElementAt(Row, Column, 0);
                      Row++;
                      grid.SetGridElementAt(Row, Column, 3);
                  }
              }
          }
          else if (direction == Direction.Undefined)
          {
              grid.SetGridElementAt(Row, Column, 3);
          
          }
      }





    /// <summary>
    /// Affiche le pacman dans la fenêtre de rendu.
    /// </summary>
    /// <param name="window">Fenêtre de rendu</param>
    public void Draw(RenderWindow window)
    {

       pacmanSprite.Position = new Vector2f(PacmanGame.DEFAULT_GAME_ELEMENT_WIDTH * Column,
                                            PacmanGame.DEFAULT_GAME_ELEMENT_HEIGHT * Row) + pacmanSprite.Origin;
            window.Draw(pacmanSprite);
    }
  }
}
