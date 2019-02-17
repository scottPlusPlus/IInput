﻿using System.Collections;
using System.Linq;
using UnityEngine;


namespace GGS.OpenInput.Test
{
    public class TestState
    {
        public TestState(MockScreenInput data)
        {
            Data = data;
        }

        public ScreenInputService ScreenInputService = new ScreenInputService();
        public ScreenInputMocker MockInputRunner = new ScreenInputMocker();
        public MockScreenInput Data;
        public bool Complete;

        public void MarkComplete()
        {
            Complete = true;
        }

        public float EndTime { get { return Data.Frames.Last().Time; } }

        public void RunToEndInEditor(int fps = 30)
        {
            ScreenInputService.SetDriver(MockInputRunner, true);
            MockInputRunner.PlayInput(Data, MarkComplete, 0f);
            float timePerTick = 1f / fps;
            float time = 0f;
            while (time <= EndTime + timePerTick)
            {
                MockInputRunner.Update(time);
                time += timePerTick;
            }
            ScreenInputService.SetDriver(null, false);
        }

        public IEnumerator RunToEndInPlay()
        {
            ScreenInputService.SetDriver(MockInputRunner, true);
            MockInputRunner.PlayInput(Data, MarkComplete, Time.time);
            while (!Complete)
            {
                MockInputRunner.Update(Time.time);
                yield return new WaitForEndOfFrame();
            }
        }

    }
}