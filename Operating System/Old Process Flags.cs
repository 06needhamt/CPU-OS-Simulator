temp.burstTime = 0;
            temp.childProcesses = new List<SimulatorProcess>();
            if (chk_Use_Default_Scheduler_Process.IsChecked != null 
                && chk_Use_Default_Scheduler_Process.IsChecked.Value) // are we using the default scheduler
            {
                temp.defaultScheduler = true;
            }
            else
            {
                temp.defaultScheduler = false;
            }
            if (chk_DelayedProcess.IsChecked != null && chk_DelayedProcess.IsChecked.Value) // does this process have a delayed start 
            {
                temp.delayedProcess = true;
                temp.delayedProcessTime = (int) cmb_Arrival_Delay.SelectedValue;
                if (rdb_DelaySecs.IsChecked != null && rdb_DelaySecs.IsChecked.Value) // if the seconds time unit was selected
                {
                    temp.delayTimeUnit = EnumTimeUnit.SECONDS;
                }
                else if (rdb_DelayTicks.IsChecked != null && rdb_DelayTicks.IsChecked.Value) // if the ticks time unit was selected
                {
                    temp.delayTimeUnit = EnumTimeUnit.TICKS;
                }
                else
                {
                    MessageBox.Show("Please Select a delay time unit"); // if no time unit was selected
                    return null;
                }
            }
            if (chk_Display_Profile.IsChecked != null && chk_Display_Profile.IsChecked.Value) // if we are displaying a performance profile
            {
                temp.displayProfile = true;
            }
            else
            {
                temp.displayProfile = false;
            }
            if (chk_Children_Die.IsChecked != null && chk_Children_Die.IsChecked.Value) // should the processes children die when it dies
            {
                temp.parentDiesChildrenDie = true;
            }
            else
            {
                temp.parentDiesChildrenDie = false;
            }
            temp.parentProcess = null;
            if (Int32.Parse(txt_ProcessLifetime.Text) == 0) // if the lifetime was 0
            {
                temp.processLifetime = Int32.MaxValue; // make it INT.MAXVALUE seconds
                temp.processLifetimeTimeUnit = EnumTimeUnit.SECONDS;
            }
            else // if the lifetime was non 0
            {
                temp.processLifetime = Int32.Parse(txt_ProcessLifetime.Text); // set the lifetime to the in-putted value
                if (rdb_LifetimeSecs.IsChecked != null && rdb_LifetimeSecs.IsChecked.Value) // if the seconds unit was selected  
                {
                    temp.processLifetimeTimeUnit = EnumTimeUnit.SECONDS; 
                }
                else if (rdb_LifetimeTicks.IsChecked != null && rdb_LifetimeTicks.IsChecked.Value) //if the ticks time unit was selected
                {
                    temp.processLifetimeTimeUnit = EnumTimeUnit.TICKS;
                }
                else
                {
                    MessageBox.Show("Please Select a Process Lifetime Time Unit"); // if no time unit is selected
                    return null;
                }
            }
            temp.processMemory = (int) cmb_Pages.SelectedValue;
            temp.processName = txtProcessName.Text;
            temp.processPriority = (int) cmb_Priority.SelectedValue;
            temp.processState = EnumProcessState.READY;
            temp.processSwapped = false;
            temp.processID = processes.Count;
            if (chk_Use_Default_Scheduler_Process.IsChecked != null 
                && chk_Use_Default_Scheduler_Process.IsChecked.Value) // should this process use the default scheduler
            {
                temp.defaultScheduler = true;
            }
            else
            {
                temp.defaultScheduler = false;
            }
            return temp; // return the created flags