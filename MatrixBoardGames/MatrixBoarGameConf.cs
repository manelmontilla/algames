using System;
using System.Collections.Generic;

namespace ALGAMES.MatrixBoardGames
{
    public class MatrixBoarGameConf
    {

        public Func<int[,], int,Tuple<int,int>[]> GetFreePositions
        {
            get;
            set;
        } = (board, NumberOfMovesDone) =>
          {
            
            //TODO:Improve: after  NumberOfMovesDone>(board.GetLength(0)* board.GetLength(1))/2 
            //start visiting de rows at board.GetLength(1)-1
            //, beacuse it will be  lesser rows to visit
             List<Tuple<int, int>> freePos = new List<Tuple<int, int>>();
             Queue<int> ColsToReviewForFreePlaces = new Queue<int>();
              if (board.GetLength(0) < 1)
              {
                 return(freePos.ToArray());    
              }

              for (int j = 0; j < board.GetLength(1); j++)
              {
                  if (board[0, j] < 0)
                  {
                      freePos.Add(new Tuple<int, int>(0, j));

                  }
                  else
                  {
                      ColsToReviewForFreePlaces.Enqueue(j);
                  }

              }

              
              int i=board.GetLength(0);
              while( i>=0 && ColsToReviewForFreePlaces.Count>0)
              {
                    var auxQueue = new Queue<int>();
                    while(ColsToReviewForFreePlaces.Count>0)
                    {
                        int col=ColsToReviewForFreePlaces.Dequeue();
                        if (board[i, col] < 0)
                        {
                            freePos.Add(new Tuple<int, int>(i, col));
                        }
                        else
                        {
                            auxQueue.Enqueue(i);
                        }
                    }
              
                   ColsToReviewForFreePlaces=auxQueue; 
              }



              return (freePos.ToArray());
          };

    }

}