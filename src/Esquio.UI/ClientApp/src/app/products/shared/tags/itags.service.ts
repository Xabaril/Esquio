import { FormTag } from './form-tag.model';
import { Tag } from './tag.model';

export interface ITagsService {
  get(productName: string, flagName: string): Promise<Tag[]>;
  add(productName: string, flagName: string, tag: Tag): Promise<void>;
  remove(productName: string, flagName: string, tag: Tag): Promise<void>;
  toFormTags(tags: Tag[]): FormTag[];
  toTags(formTags: FormTag[]): Tag[];
}
