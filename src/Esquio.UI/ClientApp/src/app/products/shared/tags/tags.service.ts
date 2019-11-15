import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { FormTag } from './form-tag.model';
import { ITagsService } from './itags.service';
import { Tag } from './tag.model';

@injectable()
export class TagsService implements ITagsService {
  // TODO: do it
  public async get(featureId: number): Promise<Tag[]> {
    const response = await fetch(`${settings.ApiUrl}/products/{productname}/features/{name}/tags`);

    if (!response.ok) {
      throw new Error('Cannot fetch tags');
    }

    return response.json();
  }

  public async add(featureId: number, tag: Tag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/{productname}/features/{name}/tags/tag`, {
      method: 'POST',
      body: JSON.stringify({
        tag: tag.name
      })
    });

    if (!response.ok) {
      throw new Error(`Cannot create tag ${tag.name}`);
    }
  }

  public async remove(featureId: number, tag: Tag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/{productname}/features/{name}/tags/untag/${tag.name}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete flag ${tag.name}`);
    }
  }

  public toFormTags(tags: Tag[]): FormTag[] {
    return tags.map(({name}) => {
      return {
        text: name
      } as FormTag;
    });
  }

  public toTags(formTags: FormTag[]): Tag[] {
    return formTags.map(({text}) => {
      return {
        name: text
      } as Tag;
    });
  }
}

