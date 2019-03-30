/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
namespace Bam.Net.Automation
{
    public interface IWorker
    {
        Job Job { get; set; }
        string Name { get; set; }
        int StepNumber { get; set; }
        bool Busy { get; set; }
        Status Status { get; set; }
        WorkState SetPropertiesFromWorkState(WorkState state);
        WorkState WorkState(WorkState state);
        WorkState Do(Job job);        
    }
}
