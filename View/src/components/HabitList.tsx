import { useState } from 'react';
import type { HabitDto, HabitRenameRequest } from "../api/habits/habits.types";
import { HabitItem } from "./HabitItem";
import { checkHabit } from '../api/habits/habits.api';

type Props = {
    habits: HabitDto[];
    onAddClick: () => void;
    onRenameSave: (renameRequest: HabitRenameRequest) => void;
    onChecked: (habitId: string, isActive: boolean) => void;
    onDelete: (habitId: string) => void;
}

export function HabitList({ habits, onAddClick, onRenameSave, onChecked, onDelete} : Props) {
    const [editingHabitId, setEditingHabitId] = useState<string | null>(null);

    return (
        <div className="habit-list-wrapper">
            <h3 className="habit-list-title">Habits for {new Date().getDate()}.{new Date().getMonth()+1}</h3>

            <div className="habit-card">
                <ul className="habit-list">
                {habits.map((habit) => (
                    <HabitItem
                        key={habit.id}
                        habit={habit}
                        isEditing={editingHabitId === habit.id}
                        onEditStart={() => setEditingHabitId(habit.id)}
                        onEditEnd={() => setEditingHabitId(null)}
                        onRenameSave={onRenameSave}
                        onChecked={onChecked}
                        onDelete={onDelete}
                    />
                ))}
                </ul>

                <button className="habit-add-btn" onClick={onAddClick}>
                    <span className="habit-add-icon">+</span>
                    Add Habit
                </button>
            </div>
        </div>
    );
}
