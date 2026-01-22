
import { useState } from 'react';

type Props = {
  habitId: string;
  initialChecked: boolean;
  onCheckChange: (checked: boolean) => void;
};

export function HabitCheackbox({ habitId, initialChecked = false, onCheckChange } : Props) {
  const [isChecked, setIsChecked] = useState(initialChecked);
  const [isLoading, setIsLoading] = useState(false);

  const handleClick = async () => {
    // Prevent multiple clicks during API call
    if (isLoading) 
      return; 
    
    setIsLoading(true);
    setIsChecked(!isChecked);
    await onCheckChange(!initialChecked); 
    setIsLoading(false);
  };

  return (
    <button
      className={`habit-checkbox ${isChecked ? 'active' : ''} ${isLoading ? 'loading' : ''}`}
      onClick={handleClick}
      aria-pressed={isChecked}
      disabled={isLoading}
    >
      {isChecked && <span className="checkmark">✓</span>}
    </button>
  );
}