using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("PublicGame")]
public class PublicGame
{
    //TODO check for GUID collision
    [DynamoDBHashKey]
    public string id { get; set; }
    [DynamoDBProperty]
    public string player1Id { get; set; }
    [DynamoDBProperty]
    public string player2Id { get; set; }
    [DynamoDBProperty]
    public List<Move> moves { get; set; }
    [DynamoDBProperty]
    public DateTime createdAt { get; set; }
    [DynamoDBProperty]
    public DateTime updatedAt { get; set; }
    [DynamoDBProperty]
    public bool active { get; set; }
    [DynamoDBVersion]
    public int? VersionNumber { get; set; }
    public PublicGame()
    {
        player1Id = null;
        player2Id = null;
        moves = new List<Move>();
        id = System.Guid.NewGuid().ToString();
        createdAt = DateTime.Now.ToUniversalTime();
        updatedAt = DateTime.Now.ToUniversalTime();
        active = true;
    }

    public int activePlayer()
    {
        return (moves.Count % 2) + 1;
    }
}