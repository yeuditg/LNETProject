import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/app.service';
import { QuestionType } from '../../types/question';
import { Answer } from '../../types/answer';

type CreateAnswerDto = { Text: string; IsCorrect: boolean };
type CreateQuestionRequest = { Text: string; Type: QuestionType; Answers: CreateAnswerDto[] };

@Component({
  selector: 'app-create',
  standalone: true,
  templateUrl:'./create.component.html',
  styleUrls: ['./create.component.css'],
  imports: [CommonModule, FormsModule]
})
export class CreateComponent {
  // עזר לגישה לEnum בטמפלט
  QuestionType = QuestionType;

  currentQuestion = {
    Text: '',
    Answers: [
      { id: 0, text: '', votes: 0, isCorrect: false },
      { id: 0, text: '', votes: 0, isCorrect: false }
    ] as Answer[],
    Type: QuestionType.Poll
  };

  allowMultipleCorrect = false;

  constructor(private api: ApiService) {}

  trackByIndex(index: number): number {
    return index;
  }

  addAnswer() {
    if (this.currentQuestion.Answers.length < 5) {
      this.currentQuestion.Answers.push({ id: 0, text: '', votes: 0, isCorrect: false });
    }
  }

  removeAnswer(index: number) {
    if (this.currentQuestion.Answers.length > 2) {
      this.currentQuestion.Answers.splice(index, 1);
    }
  }

  // מטפל בשינוי סימון תשובה כנכונה (checkbox או radio)
  onCorrectToggle(event: Event, index: number) {
    const target = event.target as HTMLInputElement;
    const checked = target.checked;

    if (this.allowMultipleCorrect) {
      this.currentQuestion.Answers[index].isCorrect = checked;
    } else {
      // Mode רדיו - רק תשובה אחת
      if (checked) {
        this.currentQuestion.Answers.forEach((answer, idx) => {
          answer.isCorrect = idx === index;
        });
      } else {
        this.currentQuestion.Answers[index].isCorrect = false;
      }
    }
  }

  createQuestion() {
    // שמירת מיפוי תשובות כולל שדה correct
    const answersPayload: CreateAnswerDto[] = this.currentQuestion.Answers
      .map((answer) => ({ Text: answer.text.trim(), IsCorrect: answer.isCorrect }))
      .filter(a => a.Text !== '');

    if (!this.currentQuestion.Text.trim()) {
      alert('נא להזין שאלה');
      return;
    }

    if (answersPayload.length < 2) {
      alert('נא להזין לפחות 2 תשובות');
      return;
    }

    if (this.currentQuestion.Type === QuestionType.Trivia ) {
      const correctCount = answersPayload.filter(a => a.IsCorrect).length;
      if (correctCount !== 1) {
        alert('בשאלת TRIVIA יש לבחור תשובה נכונה אחת בדיוק');
        return;
      }
    }     

    const data: CreateQuestionRequest = {
      Text: this.currentQuestion.Text.trim(),
      Type: this.currentQuestion.Type,
      Answers: answersPayload
    };

    this.api.createQuestion(data as any).subscribe(() => {
      alert('שאלה נוצרה בהצלחה!');
      this.currentQuestion = {
        Text: '',
        Answers: [
          { id: 0, text: '', votes: 0, isCorrect: false },
          { id: 0, text: '', votes: 0, isCorrect: false }
        ],
        Type: QuestionType.Poll
      };
    });
  }
}