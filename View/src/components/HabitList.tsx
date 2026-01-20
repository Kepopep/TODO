import type { HabitDto, HabitRenameRequest } from "../api/habits/habits.types";
import { HabitItem } from "./HabitItem";

type Props = {
    habits: HabitDto[];
    onAddClick: () => void;
    onRenameSave: (renameRequest: HabitRenameRequest) => void;
}

export function HabitList({ habits, onAddClick, onRenameSave} : Props) {
    if(habits.length === 0)
        return <div>No content</div>

    return (
        <div className="habit-list-wrapper">
            <h3 className="habit-list-title">Habits</h3>

            <div className="habit-card">
                <ul className="habit-list"> 
                {habits.map((habit) => (
                    <HabitItem key={habit.id} habit={habit} onRenameSave={onRenameSave} />
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
