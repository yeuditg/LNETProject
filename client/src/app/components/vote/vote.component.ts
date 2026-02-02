import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { ApiService } from '../../services/app.service';
import { Question, QuestionType } from '../../types/question';
import { Answer } from '../../types/answer';
import { Subject } from 'rxjs';
import { takeUntil, filter } from 'rxjs/operators';

@Component({
  selector: 'app-vote',
  standalone: true,
  templateUrl:'./vote.component.html',
  styleUrls: ['./vote.component.css'],
  imports: [CommonModule, FormsModule]
})
export class VoteComponent implements OnInit, OnDestroy {
  questions: Question[] = [];
  selectedAnswer: { [key: number]: number } = {};
  selectedQuestion: Question | null = null;
  voteResult: { answers: Answer[] } | null = null;
  loading = false;
  searchQuery = '';
  userSelectedAnswerId: number | null = null; 
  userAnswerIsCorrect: boolean | null = null; 
  QuestionType = QuestionType;
  private destroy$ = new Subject<void>();

  constructor(
    private api: ApiService, 
    private cdr: ChangeDetectorRef,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadQuestions();
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        filter((event: any) => this.route.component === VoteComponent),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.loadQuestions();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadQuestions(): void {
    this.loading = true;
    this.selectedQuestion = null;
    this.voteResult = null;
    this.userSelectedAnswerId = null;
    this.userAnswerIsCorrect = null;

    this.api.getQuestions()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: any) => {
          console.log('Questions received:', res);
          console.log('Type of res:', typeof res);
          console.log('Is array:', Array.isArray(res));
          this.questions = Array.isArray(res) ? res : (res?.data || []);
          console.log('Questions set:', this.questions);
          console.log('Questions length:', this.questions.length);
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Error loading questions:', err);
          this.questions = [];
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
  }

  selectQuestion(q: Question) {
    this.selectedQuestion = q;
    this.voteResult = null;
    this.userSelectedAnswerId = null;
    this.loading = false;
  }

  backToQuestions() {
    this.selectedQuestion = null;
    this.voteResult = null;
    this.userSelectedAnswerId = null;
    this.userAnswerIsCorrect = null;
  }

  vote(answerId: number) {
    if (!this.selectedQuestion) return;
    this.loading = true;
    this.userSelectedAnswerId = answerId; 
    console.log('Voting for answer:', answerId, 'Question:', this.selectedQuestion.id);
    this.api.vote(this.selectedQuestion.id, answerId).subscribe({
      next: (res: any) => {
        console.log('Vote response:', res);
        if (this.selectedQuestion?.type === QuestionType.Trivia) {
          this.userAnswerIsCorrect = res.isCorrect ?? null;
          console.log('User answer is correct:', this.userAnswerIsCorrect);
        }
        this.api.getResults(this.selectedQuestion!.id).subscribe({
          next: (res: any) => {
            console.log('Results response:', res);
            console.log('First answer:', res.answers[0]);
            this.voteResult = { 
              answers: Array.isArray(res) ? res : (res?.answers || res?.data || []) 
            };
            console.log('VoteResult set:', this.voteResult);
            console.log('VoteResult answers:', this.voteResult.answers);
            this.loading = false;
            this.cdr.detectChanges();
          },
          error: (err) => { 
            console.error('Error getting results:', err);
            this.loading = false; 
          }
        });
      },
      error: (err) => { 
        console.error('Error voting:', err);
        alert('שגיאה בהצבעה: ' + (err.status || 'Unknown error'));
        this.loading = false; 
      }
    });
  }

  getTotalVotes(q: Question): number {
    if (this.voteResult) {
      return this.voteResult.answers.reduce((s, a) => s + (a.votes || 0), 0);
    }
    return q.answers.reduce((s, a) => s + (a.votes || 0), 0);
  }

  getVotePercentage(votes: number, total: number): number {
    if (!total) return 0;
    return Math.round((votes / total) * 100);
  }

  getQuestionTypeLabel(t: QuestionType) {
    return t === QuestionType.Trivia ? 'טריוויה' : 'סקר';
  }

  getLetter(i: number): string {
    return String.fromCharCode(65 + i);
  }

  filteredQuestions(): Question[] {
    if (!this.searchQuery.trim()) {
      return this.questions;
    }
    const q = this.searchQuery.toLowerCase();
    return this.questions.filter(question => 
      question.text.toLowerCase().includes(q)
    );
  }

  isUserAnswer(answerId: number): boolean {
    console.log('Checking isUserAnswer - answerId:', answerId, 'userSelectedAnswerId:', this.userSelectedAnswerId, 'match:', answerId === this.userSelectedAnswerId);
    return this.userSelectedAnswerId === answerId;
  }

  isCorrectAnswer(answer: Answer): boolean {
    console.log(answer);
    
    return answer.isCorrect === true;
  }

  isWrongAnswer(answer: Answer): boolean {
    return this.isUserAnswer(answer.id) && !answer.isCorrect && this.selectedQuestion?.type === QuestionType.Trivia;
  }
}
