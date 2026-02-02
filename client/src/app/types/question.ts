import { Answer } from "./answer";

export enum  QuestionType{
Trivia =1,
Poll=2
}

export type Question = {
  id: number;
  text: string;
  type: QuestionType;
  answers: Answer[];
};

