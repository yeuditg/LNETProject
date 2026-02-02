// src/app/components/question/question.component.ts

import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Question, QuestionType } from '../../types/question';

@Component({
  selector: 'app-question',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent {
  @Input() question!: Question;
  @Input() showAnswers = true;
  @Input() clickable = false;
  @Output() questionClick = new EventEmitter<Question>();

  QuestionType = QuestionType;

  onQuestionClick() {
    if (this.clickable) {
      this.questionClick.emit(this.question);
    }
  }

  getTotalVotes(): number {
    return this.question.answers.reduce((sum: number, answer) => sum + answer.votes, 0);
  }

  getQuestionTypeLabel(): string {
    return this.question.type === QuestionType.Poll ? 'סקר' : 'טריוויה';
  }
}