import { PaginatedResponse } from '~/core';
import { Tag } from './tag.model';
import { FormTag } from './form-tag.model';

export interface ITagsService {
  get(featureId: number): Promise<Tag[]>;
  add(featureId: number, tag: Tag): Promise<void>;
  remove(featureId: number, tag: Tag): Promise<void>;
  toFormTags(tags: Tag[]): FormTag[];
  toTags(formTags: FormTag[]): Tag[];
}
