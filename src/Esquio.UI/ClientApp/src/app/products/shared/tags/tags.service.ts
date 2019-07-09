import { injectable } from 'inversify-props';
import { settings, PaginatedResponse } from '~/core';
import { ITagsService } from './itags.service';
import { Tag } from './tag.model';
import { FormTag } from './form-tag.model';

@injectable()
export class TagsService implements ITagsService {
  public async get(featureId: number): Promise<Tag[]> {
    const response = await fetch(`${settings.apiUrl}/v1/tags/${featureId}`);

    if (!response.ok) {
      throw new Error('Cannot fetch tags');
    }

    return response.json();
  }

  public async add(featureId: number, tag: Tag): Promise<void> {
    const response = await fetch(`${settings.apiUrl}/v1/tags/${featureId}`, {
      method: 'POST',
      body: JSON.stringify({
        tag: tag.name,
        featureId: tag
      }),
      headers: {
        // 'Authorization': `bearer ${token}`,
        'Content-Type': 'application/json', // TODO: interceptor
      }
    });

    if (!response.ok) {
      throw new Error(`Cannot create tag ${tag.name}`);
    }
  }

  public async remove(featureId: number, tag: Tag): Promise<void> {
    const response = await fetch(`${settings.apiUrl}/v1/tags/${featureId}/${tag.id}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete flag ${tag.id}`);
    }
  }

  public toFormTags(tags: Tag[]): FormTag[] {
    return tags.map(({id, name}) => {
      return {
        id,
        text: name
      } as FormTag;
    });
  }

  public toTags(formTags: FormTag[]): Tag[] {
    return formTags.map(({id, text}) => {
      return {
        id,
        name: text
      } as Tag;
    });
  }
}

