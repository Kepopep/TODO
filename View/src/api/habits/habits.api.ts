import type { HabitCheckRequest, HabitCreateRequest, HabitDto, HabitRenameRequest } from './habits.types';

export async function getHabits() : Promise<HabitDto[]> {
    const response = await fetch(`/api/habits?${{
        page: '1',
        pageSize: '10'
    }}`);

    if(!response.ok) {
        throw new Error('Failed to fetch habits');
    }
    
    type HabitsResponse = {
        items: HabitDto[];
    };

    const data : HabitsResponse  = await response.json();
    return data.items;
}

export async function createHabit(createRequest: HabitCreateRequest) : Promise<HabitDto> {
    const response = await fetch('/api/habits', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(createRequest)
    });

    if(response.ok === false) {
        throw new Error('Failed to create habit');
    }

    const data : HabitDto = await response.json();
    return data;
}

export async function updateHabitName(renameRequest: HabitRenameRequest) {
    const response = await fetch(`/api/habits/${renameRequest.id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            name: renameRequest.name,
            frequency: renameRequest.frequency
        })
    });

    if(response.ok === false) {
        throw new Error('Failed to update habit');
    }
}

export async function checkHabit(habitCheckRequest: HabitCheckRequest) {
    const response = await fetch(`/api/habits/${habitCheckRequest.id}/check`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            isChecked: habitCheckRequest.isChecked
        })
    });

    if (!response.ok) {
        throw new Error('Failed to update habit check status');
    }
}
