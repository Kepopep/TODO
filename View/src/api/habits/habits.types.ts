export type HabitDto = {
    id: string;
    userId: string;
    name: string;
    frequency: number;
    isChecked: boolean;
}

export type HabitCreateRequest = { 
    name: string;
    frequency: number;
}

export type HabitRenameRequest = {
    id: string;
    name: string;
    frequency: number;
}

export type HabitCheckRequest = {
    id: string;
    isChecked: boolean;
}