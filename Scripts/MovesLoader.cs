using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MovesLoader : MonoBehaviour
{
    public MoveContent move;
    public MovesDatabase db;

    public void Start()
    {

        move = new MoveContent();
        move.moveName = "Tailwhip";
        move.hpDamage = 42;
        move.hitChance = 20;
        move.spDamage = 10;
        move.critChance = 10;
        move.type = "Power";
        move.spCost = 10;
        move.cooldown = 3;
        move.range = 1;

        db.allMovesList.Add(move);

        move = new MoveContent();
        move.moveName = "Slash";
        move.hpDamage = 69;
        move.hitChance = 30;
        move.spDamage = 10;
        move.critChance = 90;
        move.type = "Power";
        move.spCost = 15;
        move.cooldown = 3;
        move.range = 1;

        db.allMovesList.Add(move);

        move = new MoveContent();
        move.moveName = "Roar";
        move.hpDamage = 300;
        move.hitChance = 25;
        move.spDamage = 100;
        move.critChance = 5;
        move.type = "Intelligence";
        move.spCost = 30;
        move.cooldown = 3;
        move.range = 1;

        db.allMovesList.Add(move);
    }
}
