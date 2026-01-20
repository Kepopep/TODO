import "../components/style/HabitList.css";
import "../components/style/HabitItem.css";
import '../components/style/HabitCheackbox.css'

import { useEffect, useState } from "react";
import { createHabit, getHabits, updateHabitName} from "../api/habits/habits.api";
import { HabitList } from "../components/HabitList";

import type { HabitDto, HabitRenameRequest } from "../api/habits/habits.types";

export function HabitPage() {
    const [habits, setHabits] = useState<HabitDto[]>([]); 
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        loadHabits();
    }, []);

    async function loadHabits() {
        try {
            const data = await getHabits();
            setHabits(data);
        } catch {
            setError("Failed to fetch habits");
        } finally {
            setIsLoading(false);
        }
    }
    
    async function addHabit() {
        const data = await createHabit({ 
            name: "New Habit",
            frequency: 0
        });
    
        await loadHabits();
    }

    async function updateName(renameRequest : HabitRenameRequest) {
        await updateHabitName({ 
            id: renameRequest.id,
            name: renameRequest.name,
            frequency: renameRequest.frequency
        });
        
        await loadHabits();
    }

    if(isLoading) 
        return <div>Loading...</div>

    if(error)
        return <div>{error}</div>

    return <HabitList habits={habits} onAddClick={addHabit} onRenameSave={updateName}/>
}