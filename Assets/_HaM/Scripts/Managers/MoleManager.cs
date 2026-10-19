using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoleManager : MonoBehaviour
{
    [SerializeField] private Mole[] moles;
    [SerializeField] private float raiseTime = 2f;

    private Dictionary<Mole, Coroutine> raisedMoleCoroutines = new();
    private bool isGameActive;
    
    private void Start()
    {
        EventManager.StartListening(Consts.Events.Game.Started, GameStarted);
        EventManager.StartListening(Consts.Events.Game.Ended, GameEnded);
        EventManager.StartListening(Consts.Events.Mole.Hit, MoleHit);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Started, GameStarted);
        EventManager.StopListening(Consts.Events.Game.Ended, GameEnded);
        EventManager.StopListening(Consts.Events.Mole.Hit, MoleHit);
    }

    private void GameStarted()
    {
        isGameActive = true;
        
        RaiseRandomMole();
    }
    
    private void GameEnded()
    {
        isGameActive = false;
        
        foreach (var raisedMoleCoroutine in raisedMoleCoroutines)
        {
            raisedMoleCoroutine.Key.Lower();
            StopCoroutine(raisedMoleCoroutine.Value);
        }
        
        raisedMoleCoroutines.Clear();
    }

    private void MoleHit(object param)
    {
        if (param is not Mole mole)
            return;

        if(!raisedMoleCoroutines.TryGetValue(mole, out var coroutine))
            return;
        
        StopCoroutine(coroutine);
        RaiseRandomMole();
        
        raisedMoleCoroutines.Remove(mole);
    }

    private void RaiseRandomMole()
    {
        if (!isGameActive)
            return;
        
        var mole = GetRandomMole();
        mole.Raise();

        var coroutine = StartCoroutine(MoleMissedCoroutine(mole));
        raisedMoleCoroutines.Add(mole, coroutine);
    }

    private IEnumerator MoleMissedCoroutine(Mole mole)
    {
        yield return new WaitForSeconds(raiseTime);
        
        mole.Lower();
        EventManager.TriggerEvent(Consts.Events.Mole.Missed);
        
        RaiseRandomMole();
        raisedMoleCoroutines.Remove(mole);
    }
    
    private Mole GetRandomMole()
    {
        var availableMoles = moles
            .Where(mole => !raisedMoleCoroutines.ContainsKey(mole))
            .ToArray();

        if (availableMoles.Length == 0)
            return null;

        return availableMoles[Random.Range(0, availableMoles.Length)];
    }
}
