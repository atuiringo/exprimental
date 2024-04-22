using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PlayerAttackStream
{

    public Action OnFirstAttackAction;
    public Action OnSecondAttackAction;

    public ReactiveProperty<bool> OnAttackSignal;
    public float FIRST_ATTACK_ROCK_SECONDS = 0.25f;
    public float SECOND_ATTACK_ROCK_SECONDS = 0.42f;
    public float SECOND_ATTACK_START_INTERVAL_SECONDS = 0.01f;

    ReactiveProperty<int> _onAttackSignalCount;
    ReactiveProperty<bool> _onFirstAttack;
    ReactiveProperty<bool> _onSecondAttack;


    public PlayerAttackStream()
    {
        OnAttackSignal = new ReactiveProperty<bool>(false);

        _onFirstAttack = new ReactiveProperty<bool>(false);
        _onSecondAttack = new ReactiveProperty<bool>(false);
        _onAttackSignalCount = new ReactiveProperty<int>(0);

        OnAttackSignal.Subscribe(_ =>
        {
            if (_)
            {
                _onAttackSignalCount.Value++;
            }
        });

        _onFirstAttack.Subscribe(_ =>
        {
            if (_) OnFirstAttackAction();
        });

        _onSecondAttack.Subscribe(_ =>
        {
            if (_) OnSecondAttackAction();
        });



        OnAttackSignal.Subscribe(_ =>
        {
            if (_onAttackSignalCount.Value == 1)
            {
                _onFirstAttack.Value = true;

                Observable
                .Timer(TimeSpan.FromSeconds(FIRST_ATTACK_ROCK_SECONDS - SECOND_ATTACK_START_INTERVAL_SECONDS))
                .Do(_ =>
                {
                    if (_onAttackSignalCount.Value == 2)
                    {
                        _onSecondAttack.Value = true;
                        Observable
                        .Timer(TimeSpan.FromSeconds(SECOND_ATTACK_ROCK_SECONDS))
                        .Subscribe(_ =>
                        {
                            _onFirstAttack.Value = false;
                            _onSecondAttack.Value = false;
                            _onAttackSignalCount.Value = 0;
                        });

                    };
                })
                .Delay(TimeSpan.FromSeconds(SECOND_ATTACK_START_INTERVAL_SECONDS))
                .Subscribe(_ =>
                {
                    if (_onAttackSignalCount.Value == 1)
                    {
                        _onFirstAttack.Value = false;
                        _onAttackSignalCount.Value = 0;
                    }
                });
            }
        });
    }
}
