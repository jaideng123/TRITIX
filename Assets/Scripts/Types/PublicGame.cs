using System;
using System.Collections.Generic;

public class PublicGame
{
    public string gameId { get; set; }
    public string player1Id { get; set; }
    public string player2Id { get; set; }
    public List<Move> moves { get; set; }
    public PublicGame()
    {
        player1Id = null;
        player2Id = null;
        moves = new List<Move>();
    }
    public int activePlayer()
    {
        return (moves.Count % 2) + 1;
    }
}