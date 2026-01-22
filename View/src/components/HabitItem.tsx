import { useState } from 'react';
import type { HabitDto, HabitRenameRequest } from '../api/habits/habits.types';
import { HabitCheackbox } from './HabitCheackbox';
import { HabitEdit } from './HabitEdit';

type Props = {
  habit: HabitDto;
  isEditing?: boolean;
  onEditStart?: () => void;
  onEditEnd?: () => void;
  onRenameSave: (renameRequest: HabitRenameRequest) => void;
  onChecked: (habitId: string, isActive: boolean) => void;
};

export function HabitItem({ habit, isEditing = false, onEditStart, onEditEnd, onRenameSave, onChecked}: Props) {
  const [isUpdating, setIsUpdating] = useState(false);

  return (
    <li className="habit-item">
      {isUpdating ? (
        <div className='loader'/>
      ) : isEditing ? (
        <HabitEdit
          initialValue = {habit.name}
          onSave={async (name) => {
            console.log(name);

            setIsUpdating(true);
            await onRenameSave({
              id: habit.id,
              name: name,
              frequency: habit.frequency
            });
            setIsUpdating(false);

            if (onEditEnd) {
              onEditEnd();
            }
          }}
          onCancel={() => {
            if (onEditEnd) {
              onEditEnd();
            }
          }}/>
      ) : (
        <>
          <HabitCheackbox 
            habitId={habit.id} 
            initialChecked={habit.isChecked} 
            onCheckChange={(checked) => onChecked(habit.id, checked)}/>
          <span className="habit-icon">🌿</span>
          <span className="habit-name">{habit.name}</span>
          <button className="habit-item-button" onClick={() => {
            if (onEditStart) {
              onEditStart();
            }
          }}>⋯</button>
        </>
      )}
      </li>);
}